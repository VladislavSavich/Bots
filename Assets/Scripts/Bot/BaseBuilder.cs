using System;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    public event Action<Base> BaseBuilded;

    public void Build() 
    {
        Base building = Instantiate(_prefab);
        building.transform.position = transform.position;
        building.transform.SetParent(transform.parent);
        building.Initialize();
        BaseBuilded?.Invoke(building);
    }
}
