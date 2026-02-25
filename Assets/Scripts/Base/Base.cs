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
    [SerializeField] private FlagMaker _flagMaker;
    [SerializeField] private int _maximumBotCount;

    private int _barrelsForSpawn = 3;
    private int _barrelsForBase = 5;
    private int _minBotForColonization = 1;
    private bool _isColonizationStarted = false;
    private WaitForSeconds _delay = new WaitForSeconds(0.1f);
    private Coroutine _mainCoroutine;

    private void Awake()
    {
        transform.SetParent(transform);
    }

    private void Start()
    {
        Initialize();

        _storage.OnBotAdded += SubscribeToBot;
        _scanner.BarrelDetected += _barrelService.AddTask;
        _barrelService.TaskReceived += StartTask;
        _counter.CountChanged += CheckNumbersOfBot;

        _mainCoroutine = StartCoroutine(TaskLoop());
    }

    public void Initialize()
    {
        _barrelService = GetComponentInParent<BarrelService>();
        _barrelSpawner = GetComponentInParent<BarrelSpawner>();
        _botService = GetComponentInParent<BotService>();
    }

    private void OnDisable()
    {
        _storage.OnBotAdded -= SubscribeToBot;
        _scanner.BarrelDetected -= _barrelService.AddTask;
        _barrelService.TaskReceived -= StartTask;
        _counter.CountChanged -= CheckNumbersOfBot;
    }

    private void CheckNumbersOfBot()
    {
        if (!_flagMaker.IsFlagStand && _storage.NumberOfBots < _maximumBotCount && _counter.BarrelsCount >= _barrelsForSpawn)
        {
            _botSpawner.SpawnObject();
            _counter.ReduceCount(_barrelsForSpawn);
        }
    }

    private void StartTask()
    {
        while (_botService.HasFreeBots(_storage.GetAllBots()) && _barrelService.HasTasks)
        {
            if (_botService.TryGetFreeBot(_storage.GetAllBots(), out Bot bot))
            {
                if (_flagMaker.IsFlagStand && _counter.BarrelsCount >= _barrelsForBase && !_isColonizationStarted && _storage.NumberOfBots > _minBotForColonization)
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

            StartTask();
        }
    }

    private void CompleteColonization(Bot bot)
    {
        _flagMaker.DestroyFlag();
        _flagMaker.StopWaitingCoroutine();
        _isColonizationStarted = false;
        UnsubscribeToBot(bot);
        bot.SubscribeToNewBase();
        _counter.ReduceCount(_barrelsForBase);
    }

    private void TaskComplete(Barrel barrel, Bot bot)
    {
        _barrelService.CompleteTask(barrel);
        _barrelSpawner.ReleaseObject(barrel);
        _counter.AddCount();
    }

    public void StartColonization()
    {
        _flagMaker.DestroyFlag();
        _flagMaker.StopWaitingCoroutine();
        _flagMaker.StartWaitingForFlag();
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
        }
    }
}
