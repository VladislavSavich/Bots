using System;
using System.Collections.Generic;
using UnityEngine;

public class BotStorage : MonoBehaviour
{
    private List<Bot> _bots = new List<Bot>();

    public event Action<Bot> OnBotAdded;

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
        OnBotAdded?.Invoke(bot);
    }

    public List<Bot> GetAllBots()
    {
        return new List<Bot>(_bots);
    }
}
