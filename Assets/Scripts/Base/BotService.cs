using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BotService : MonoBehaviour
{
    public bool TryGetFreeBot(List<Bot> bots, out Bot bot)
    {
        bot = bots.FirstOrDefault(b => b != null && !b.IsActive);

        return bot != null;
    }

    public bool HasFreeBots(List<Bot> bots)
    {
        var freeBots = bots.Where(b => b != null && !b.IsActive).ToList();

        return freeBots.Count > 0;
    }
}
