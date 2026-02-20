using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BotStorage _storage;
    [SerializeField] private BotService _botService;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private BarrelService _barrelService;
    [SerializeField] private BarrelSpawner _barrelSpawner;
    [SerializeField] private BarrelScanner _scanner;
    [SerializeField] private BarrelCounter _counter;
    [SerializeField] private ClickHandler _clickHandler;
    [SerializeField] private FlagMaker _flagMaker;
    [SerializeField] private int _maximumBotCount;

    private int _minBotForColonization = 1;
    private bool _isColonizationStarted = false;
    private WaitForSeconds _delay = new WaitForSeconds(0.1f);
    private Coroutine _mainCoroutine;

    private void Awake()
    {
        if (_barrelSpawner == null)
            _barrelSpawner = FindObjectOfType<BarrelSpawner>();

        if (_barrelService == null)
            _barrelService = FindObjectOfType<BarrelService>();

        if (_botService == null)
            _botService = FindObjectOfType<BotService>();
    }

    private void Start()
    {
        _mainCoroutine = StartCoroutine(TaskLoop());
    }

    private void OnEnable()
    {
        _storage.OnBotAdded += SubscribeToBot;
        _scanner.BarrelDetected += _barrelService.AddTask;
        _barrelService.TaskReceived += StartTask;
        _counter.EnoughBarrelsForSpawn += CheckNumbersOfBot;
        _clickHandler.BaseClicked += _flagMaker.StartWaitingForFlag;
    }

    private void OnDisable()
    {
        _storage.OnBotAdded -= SubscribeToBot;
        _scanner.BarrelDetected -= _barrelService.AddTask;
        _barrelService.TaskReceived -= StartTask;
        _counter.EnoughBarrelsForSpawn -= CheckNumbersOfBot;
        _clickHandler.BaseClicked -= _flagMaker.StartWaitingForFlag;
    }

    private void CheckNumbersOfBot()
    {
        if (!_flagMaker.IsFlagStand && _storage.NumberOfBots < _maximumBotCount)
        {
            _botSpawner.SpawnObject();
            _counter.ReduceCount(_counter.BarrelsForBot);
        }
    }

    private void StartTask()
    {
        while (_botService.HasFreeBots && _barrelService.HasTasks)
        {
            if (_botService.TryGetFreeBot(out Bot bot))
            {
                if (_flagMaker.IsFlagStand && _counter.IsEnoughForBase && !_isColonizationStarted && _storage.NumberOfBots > _minBotForColonization)
                {
                    bot.SetFlagPosition(_flagMaker.FlagPosition);
                    _isColonizationStarted = true;
                }
                else
                {
                    if (_barrelService.TryGetTask(out Barrel barrel))
                    {
                        bot.SetTarget(barrel);
                    }
                    else
                    {
                        _botService.AddFreeBot(bot);
                        continue;
                    }
                }
            }
            else
            {
                continue;
            }
        }
    }

    private IEnumerator TaskLoop()
    {
        while (enabled)
        {
            yield return _delay;

            _botService.FindFreeBot(_storage.GetAllBots());

            StartTask();
        }
    }

    private void CompleteColonization(Bot bot)
    {
        _flagMaker.DestroyFlag();
        _isColonizationStarted = false;
        UnsubscribeToBot(bot);
        bot.SubscribeToNewBase();
        _counter.ReduceCount(_counter.BarrelsForBase);
    }

    private void TaskComplete(Barrel barrel, Bot bot)
    {
        _barrelService.CompleteTask(barrel);
        _barrelSpawner.ReleaseObject(barrel);
        _counter.AddCount();
        _botService.AddFreeBot(bot);
    }

    public void UnsubscribeToBot(Bot bot)
    {
        bot.BarrelWasDelivered -= TaskComplete;
        bot.BaseWasBuilded -= CompleteColonization;
        _storage.RemoveBot(bot);
    }

    public void SubscribeToBot(Bot bot)
    {
        if (_storage.ContainsBot(bot))
        {
            bot.BarrelWasDelivered += TaskComplete;
            bot.BaseWasBuilded += CompleteColonization;
        }
        else
        {
            _storage.AddBot(bot);
            _botService.AddFreeBot(bot);
        }
    }
}
