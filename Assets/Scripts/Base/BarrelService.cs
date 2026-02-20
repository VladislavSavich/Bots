using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrelService : MonoBehaviour
{
    private List<Barrel> _foundedBarrels = new List<Barrel>();
    private List<Barrel> _occupiedBarrels = new List<Barrel>();

    public bool HasTasks => _foundedBarrels.Count > 0;

    public event Action TaskReceived;

    public void AddTask(Barrel barrel)
    {
        if (barrel != null && !_occupiedBarrels.Contains(barrel) && !_foundedBarrels.Contains(barrel))
        {
            _foundedBarrels.Add(barrel);
            TaskReceived?.Invoke();
        }
    }

    public bool TryGetTask(out Barrel barrel)
    {
        barrel = null;

        if (_foundedBarrels.Count == 0)
            return false;

        barrel = _foundedBarrels[0];
        _foundedBarrels.RemoveAt(0);

        _occupiedBarrels.Add(barrel);
        return true;
    }

    public void CompleteTask(Barrel barrel)
    {
        if (barrel != null)
        {
            _occupiedBarrels.Remove(barrel);
        }
    }
}
