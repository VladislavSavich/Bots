using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _prefab;
    [SerializeField] private int _initialBotCount;
    [SerializeField] private Collider _spawnArea;
    [SerializeField] private BotStorage _storage;

    private void Start()
    {
        if (_initialBotCount > 0)
        {
            for (int i = 0; i < _initialBotCount; i++)
            {
                SpawnObject();
            }
        }
    }

    public void SpawnObject()
    {
        Bot bot = Instantiate(_prefab);
        bot.transform.position = transform.position;
        _storage.AddBot(bot);
    }
}
