using System;
using UnityEngine;

public class BarrelCounter : MonoBehaviour
{
    private int _minValue = 0;
    private int _count;

    public event Action<int> CountChanged;

    private void Start()
    {
        ResetCounter();
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
    }
}
