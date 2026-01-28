using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BotManager _manager;
    [SerializeField] private BarrelDetector _detector;
    [SerializeField] private BarrelCounter _counter;

    private void OnEnable()
    {
        _detector.BarrelDetected += _manager.StartTask;
        _detector.BarrelFoundNearby += _counter.AddCount;
    }

    private void OnDisable()
    {
        _detector.BarrelDetected -= _manager.StartTask;
    }
}
