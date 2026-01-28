using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : Spawner<Bot>
{
    private List<Bot> _bots = new List<Bot>();

    protected override void SpawnObject()
    {
        if (Pool.CountActive < PoolMaxSize)
        {
            Bot bot = Pool.Get();
            bot.transform.localPosition = GenerateRandomPosition();
            _bots.Add(bot);
        }
    }

    protected override Vector3 GenerateRandomPosition()
    {
        Bounds bounds = _spawnArea.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.min.z)
        );
    }

    public List<Bot> GetAllBots()
    {
        return new List<Bot>(_bots);
    }
}
