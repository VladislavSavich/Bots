using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private Barrel _prefab;
    [SerializeField] private int _poolCapacity = 8;
    [SerializeField] private int _poolMaxSize = 8;
    [SerializeField] private Collider _spawnArea;

    private ObjectPool<Barrel> _pool;
    private WaitForSeconds _respawnTime = new WaitForSeconds(2);
    private Coroutine _respawnCoroutine;

    private void Awake()
    {
        _pool = new ObjectPool<Barrel>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (barrel) => ActionOnGet(barrel),
        actionOnRelease: (barrel) => ActionOnRelease(barrel),
        actionOnDestroy: (barrel) => Destroy(barrel),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        for (int i = 0; i < _poolCapacity; i++)
        {
            SpawnObject();
        }

        _respawnCoroutine = StartCoroutine(SpawnObjectRoutine());
    }

    private void ActionOnGet(Barrel barrel)
    {
        barrel.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Barrel barrel)
    {
        barrel.gameObject.SetActive(false);
    }

    public void ReleaseObject(Barrel barrel)
    {
        if (barrel != null && barrel.gameObject.activeInHierarchy)
        {
            _pool.Release(barrel);
        }
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (enabled)
        {
            yield return _respawnTime;

            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (_pool.CountActive < _poolMaxSize)
        {
            Barrel barrel = _pool.Get();

            barrel.transform.position = GenerateRandomPosition();
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        Bounds bounds = _spawnArea.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}