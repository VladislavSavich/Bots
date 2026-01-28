using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected int PoolCapacity = 8;
    [SerializeField] protected int PoolMaxSize = 8;
    [SerializeField] protected Collider _spawnArea;

    protected ObjectPool<T> Pool;
    protected WaitForSeconds _respawnTime = new WaitForSeconds(2);
    protected Coroutine _respawnCoroutine;

    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>(
        createFunc: () => Instantiate(Prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => ActionOnRelease(obj),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: PoolCapacity,
        maxSize: PoolMaxSize);
    }

    protected virtual void Start()
    {
        for (int i = 0; i < PoolCapacity; i++)
        {
            SpawnObject();
        }

        _respawnCoroutine = StartCoroutine(SpawnObjectRoutine());
    }

    protected virtual void ActionOnGet(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void ActionOnRelease(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected virtual void ReleaseObject(T obj)
    {
        Pool.Release(obj);
    }

    protected virtual IEnumerator SpawnObjectRoutine()
    {
        while (enabled)
        {
            yield return _respawnTime;

            SpawnObject();
        }
    }

    protected virtual void SpawnObject()
    {
        if (Pool.CountActive < PoolMaxSize)
        {
            T obj = Pool.Get();

            obj.transform.position = GenerateRandomPosition();
        }
    }

    protected virtual Vector3 GenerateRandomPosition()
    {
        Bounds bounds = _spawnArea.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}