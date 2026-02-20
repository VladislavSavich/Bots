using System.Collections.Generic;
using UnityEngine;

public class BotService : MonoBehaviour
{
    private Queue<Bot> _freeBots = new Queue<Bot>();

    public bool HasFreeBots => _freeBots.Count > 0;

    public bool TryGetFreeBot(out Bot bot)
    {
        bot = null;

        if (_freeBots.Count == 0)
            return false;

        bot = _freeBots.Dequeue();
        return true;
    }

    public void AddFreeBot(Bot bot)
    {
        if (bot != null && !_freeBots.Contains(bot))
        {
            _freeBots.Enqueue(bot);
        }
    }

    public void FindFreeBot(List<Bot> bots)
    {
        for (int i = 0; i < bots.Count; i++)
        {
            if (bots[i] != null && !bots[i].IsActive && !_freeBots.Contains(bots[i]))
            {
                _freeBots.Enqueue(bots[i]);
            }
        }
    }

    public void RemoveBot(Bot bot)
    {
        var tempList = new List<Bot>();

        while (_freeBots.Count > 0)
        {
            var b = _freeBots.Dequeue();

            if (b != bot)
                tempList.Add(b);
        }

        foreach (var b in tempList)
            _freeBots.Enqueue(b);
    }
}
