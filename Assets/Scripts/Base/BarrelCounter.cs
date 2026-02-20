using System;
using UnityEngine;

public class BarrelCounter : MonoBehaviour
{
    private int _barrelsForSpawn = 3;
    private int _barrelsForBase = 5;
    private int _minValue = 0;
    private int _count;

    public event Action<int> CountChanged;
    public event Action EnoughBarrelsForSpawn;

    public int BarrelsForBot { get; private set; }
    public int BarrelsForBase { get; private set; }
    public bool IsEnoughForBase { get; private set; }

    private void Start()
    {
        ResetCounter();
        BarrelsForBot = _barrelsForSpawn;
        BarrelsForBase = _barrelsForBase;
    }

    private void ResetCounter()
    {
        _count = _minValue;
        CountChanged?.Invoke(_count);
    }

    public void AddCount()
    {
        _count++;
        CountChanged?.Invoke(_count);

        if (_count >= _barrelsForSpawn)
            EnoughBarrelsForSpawn?.Invoke();

        if (_count >= _barrelsForBase)
        {
            IsEnoughForBase = true;
        }
        else
        {
            IsEnoughForBase = false;
        }
    }

    public void ReduceCount(int value)
    {
        if (_count >= value)
        {
            _count -= value;
            CountChanged?.Invoke(_count);
        }
    }
}
