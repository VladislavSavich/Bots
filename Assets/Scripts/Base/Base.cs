using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BotStorage _storage;
    [SerializeField] private BarrelSpawner _spawner;
    [SerializeField] private BarrelScanner _scanner;
    [SerializeField] private BarrelCounter _counter;

    private WaitForSeconds _delay = new WaitForSeconds(0.1f);
    private Queue<Barrel> _foundedBarrels = new Queue<Barrel>();
    private List<Barrel> _occupiedBarrels = new List<Barrel>();
    private Queue<Bot> _freeBots = new Queue<Bot>();
    private List<Bot> _subscribedBots = new List<Bot>();
    private Coroutine _waitingCoroutine;

    private void OnEnable()
    {
        _storage.OnBotAdded += SubscribeToBot;
        _scanner.BarrelDetected += AddTask;
    }

    private void OnDisable()
    {
        UnsubscribeFromAllBots();
        _scanner.BarrelDetected -= AddTask;
    }

    private void SubscribeToBot(Bot bot)
    {
        bot.BarrelWasDelivered += TaskComplete;
        _subscribedBots.Add(bot);

        if (!bot.IsActive && !_freeBots.Contains(bot))
        {
            _freeBots.Enqueue(bot);
        }
    }

    private void UnsubscribeFromAllBots()
    {
        foreach (Bot bot in _subscribedBots)
        {
            bot.BarrelWasDelivered -= TaskComplete;
        }

        _subscribedBots.Clear();
    }

    private void AddTask(Barrel barrel)
    {
        if (barrel != null && !_occupiedBarrels.Contains(barrel) && !_foundedBarrels.Contains(barrel))
        {
            _foundedBarrels.Enqueue(barrel);

            StartTask();
        }
    }

    private void StartTask()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        while (_freeBots.Count > 0 && _foundedBarrels.Count > 0)
        {
            Barrel barrel = _foundedBarrels.Peek();
            Bot bot = _freeBots.Peek();

            if (barrel == null)
            {
                _foundedBarrels.Dequeue();
                continue;
            }

            if (bot == null || bot.IsActive)
            {
                _freeBots.Dequeue();
                continue;
            }

            barrel = _foundedBarrels.Dequeue();
            bot = _freeBots.Dequeue();

            _occupiedBarrels.Add(barrel);
            bot.SetTarget(barrel);
        }

        if (_foundedBarrels.Count > 0)
            _waitingCoroutine = StartCoroutine(SearchingFreeBot());
    }

    private IEnumerator SearchingFreeBot()
    {
        while (enabled)
        {
            yield return _delay;

            FindFreeBot();
        }
    }

    private void FindFreeBot()
    {
        List<Bot> bots = _storage.GetAllBots();

        for (int i = 0; i < bots.Count; i++)
        {
            if (bots[i] != null && !bots[i].IsActive && !_freeBots.Contains(bots[i]))
            {
                _freeBots.Enqueue(bots[i]);
            }
        }
    }

    private void TaskComplete(Barrel barrel, Bot bot)
    {
        _occupiedBarrels.Remove(barrel);
        _spawner.ReleaseObject(barrel);
        _counter.AddCount();
        _freeBots.Enqueue(bot);
    }
}
