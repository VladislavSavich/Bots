using System;
using System.Collections;
using UnityEngine;

public class BarrelScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 20f;
    [SerializeField] private LayerMask _barrelLayer;

    private int _maximumTargets = 20;
    private Collider[] _targets;
    private WaitForSeconds _delay = new WaitForSeconds(2);

    public event Action<Barrel> BarrelDetected;

    private void Start()
    {
        _targets = new Collider[_maximumTargets];
        StartCoroutine(Scaning());
    }

    private IEnumerator Scaning()
    {
        while (enabled)
        {
            yield return _delay;

            ScanZone();
        }
    }

    private void ScanZone()
    {
         int _targetCount = Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _targets, _barrelLayer);

        for (int i = 0; i < _targetCount; i++)
        {
            if (_targets[i].TryGetComponent(out Barrel barrel))
            {
                BarrelDetected?.Invoke(barrel);
            }
        }
    }
}
