using System;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private bool _inUse = false;
    private bool _inGrab = false;

    public event Action<Barrel> OnCollected;
    public Vector3 Position => transform.position;
    public bool InProgress => _inUse;
    public bool InTakeOver => _inGrab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Base _))
        {
            OnCollected?.Invoke(this);
            transform.SetParent(null);
        }
    }

    public void BecomeTask()
    {
        _inUse = true;
    }

    public void Become—aptured()
    {
        _inGrab = true;
    }

    public void ResetCondition()
    {
        _inUse = false;
        _inGrab = false;
    }
}
