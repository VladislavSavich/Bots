using System;
using System.Collections.Generic;
using UnityEngine;

public class BotStorage : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();

    public int NumberOfBots => _bots.Count;

    public event Action<Bot> OnBotAdded;

    public bool ContainsBot(Bot bot)
    {
        return _bots.Contains(bot);
    }

    public void AddBot(Bot bot)
    {
        if (!_bots.Contains(bot))
        {
            _bots.Add(bot);
            OnBotAdded?.Invoke(bot);
        }
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
    }

    public List<Bot> GetAllBots()
    {
        return new List<Bot>(_bots);
    }
}
