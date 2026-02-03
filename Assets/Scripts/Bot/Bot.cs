using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _mover;
    [SerializeField] private BotAnimator _animator;
    [SerializeField] private Grabber _grabber;

    private Barrel _targetBarrel;

    public event Action<Barrel, Bot> BarrelWasDelivered;
    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        _mover.BarrelReached += OnBarrelReached;
        _mover.BaseReached += OnBaseReached;
    }

    private void OnDisable()
    {
        _mover.BarrelReached -= OnBarrelReached;
        _mover.BaseReached -= OnBaseReached;
    }

    private void OnBarrelReached()
    {
        _mover.StopMoving();
        _grabber.PickUp(_targetBarrel);
        _animator.SetupWalkWithBarrel();
        _mover.MoveToBase();
    }

    private void OnBaseReached()
    {
        Barrel deliveredBarrel = _targetBarrel;
        _targetBarrel = null;
        deliveredBarrel.transform.SetParent(null);
        _mover.StopMoving();
        _animator.SetupStaticIdle();
        BarrelWasDelivered?.Invoke(deliveredBarrel, this);
        IsActive = false;
    }

    public void SetTarget(Barrel barrel)
    {
        if (barrel != null)
        {
            IsActive = true;
            _targetBarrel = barrel;
            _mover.MoveToBarrel(barrel.transform.position);
            _animator.SetupWalk();
        }
    }
}
