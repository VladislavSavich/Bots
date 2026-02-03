using System;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public event Action BarrelWasGrab;

    public void PickUp(Barrel barrel)
    {
        if (barrel != null)
        {
            barrel.transform.SetParent(transform);
            BarrelWasGrab?.Invoke();
        }
    }
}
