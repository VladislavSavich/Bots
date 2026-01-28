using System;
using System.Collections;
using UnityEngine;

public class BarrelDetector : MonoBehaviour
{
    [SerializeField] private bool _isScanActive = false;
    [SerializeField] private float _scanRadius = 20f;

    private int _maximumTargets = 20;
    private int _targetCount;
    private Collider[] _targets;
    private WaitForSeconds _delay = new WaitForSeconds(2);

    public event Action<Barrel> BarrelDetected;
    public event Action<Barrel> BarrelFoundNearby;

    private void Start()
    {
        if (_isScanActive)
        {
            _targets = new Collider[_maximumTargets];
            StartCoroutine(Scaning());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Barrel barrel))
            BarrelFoundNearby?.Invoke(barrel);
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
        _targetCount = Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _targets);

        for (int i = 0; i < _targetCount; i++)
        {
            Collider collider = _targets[i];

            if (collider.TryGetComponent(out Barrel barrel) && !barrel.InProgress)
            {
                BarrelDetected?.Invoke(barrel);
            }
        }
    }
}
