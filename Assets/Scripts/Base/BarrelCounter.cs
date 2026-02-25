using System;
using UnityEngine;

public class BarrelCounter : MonoBehaviour
{
    private int _minValue = 0;
    private int _count;

    public event Action CountChanged;

    public int BarrelsCount => _count;

    private void Start()
    {
        ResetCounter();
    }

    private void ResetCounter()
    {
        _count = _minValue;
        CountChanged?.Invoke();
    }

    public void AddCount()
    {
        _count++;
        CountChanged?.Invoke();
    }

    public void ReduceCount(int value)
    {
        if (_count >= value)
        {
            _count -= value;
            CountChanged?.Invoke();
        }
    }
}
