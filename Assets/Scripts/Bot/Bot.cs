using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _mover;
    [SerializeField] private BotAnimator _animator;
    [SerializeField] private BarrelDetector _detector;
    [SerializeField] private Grabber _grabber;

    private Barrel _targetBarrel;
    private Vector3 _startPosition;
    private bool _isBusy;

    public bool IsActive => _isBusy;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        _detector.BarrelFoundNearby += CheckTarget;
        _mover.ReachedTheTarget += ResetTarget;
        _grabber.BarrelWasGrab += GoBack;
    }

    private void OnDisable()
    {
        _detector.BarrelFoundNearby -= CheckTarget;
        _mover.ReachedTheTarget -= ResetTarget;
        _grabber.BarrelWasGrab -= GoBack;
    }

    private void CheckTarget(Barrel barrel)
    {
        if (barrel == _targetBarrel)
            _grabber.PickUp(barrel);
    }

    private void GoBack()
    {
        _isBusy = true;
        _mover.StartMoving(_startPosition);
        _animator.SetupWalkWithBarrel();
    }

    private void ResetTarget()
    {
        _isBusy = false;
        _targetBarrel = null;
        _mover.StopMoving();
        _animator.SetupStaticIdle();
    }

    public void SetTarget(Barrel barrel)
    {
        if (!barrel.InProgress)
        {
            _isBusy = true;
            _targetBarrel = barrel;
            barrel.BecomeTask();
            _mover.StartMoving(barrel.transform.position);
            _animator.SetupWalk();
        }
    }
}
