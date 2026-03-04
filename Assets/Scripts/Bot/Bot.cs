using System;
using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _mover;
    [SerializeField] private BotAnimator _animator;
    [SerializeField] private Grabber _grabber;
    [SerializeField] private BaseBuilder _builder;

    private float _subscribeDelay = 1f;
    private Barrel _targetBarrel;
    private Base _newBase;
    private bool _isMovingToBase;
    private bool _isMovingToFlag;

    public event Action<Barrel, Bot> BarrelWasDelivered;
    public event Action<Bot> BaseWasBuilded;

    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        _mover.TargetReached += CheckTarget;
        _builder.BaseBuilded += SetNewBase;
    }

    private void OnDisable()
    {
        _mover.TargetReached -= CheckTarget;
        _builder.BaseBuilded -= SetNewBase;
    }

    public void SetTarget(Barrel barrel)
    {
        if (barrel != null)
        {
            IsActive = true;
            _targetBarrel = barrel;
            _isMovingToFlag = false;
            _isMovingToBase = false;
            _mover.StartMoving(barrel.transform.position);
            _animator.SetupWalk();
        }
    }

    public void SetFlagPosition(Vector3 position)
    {
        IsActive = true;
        _isMovingToBase = false;
        _isMovingToFlag = true;
        _mover.StartMoving(position);
        _animator.SetupWalk();
    }

    public void SubscribeWithDelay()
    {
        Invoke(nameof(SubscribeToNewBase), _subscribeDelay);
    }

    private void CheckTarget()
    {
        if (_isMovingToBase)
        {
            OnBaseReached();
        }
        else if (_isMovingToFlag)
        {
            OnFlagReached();
        }
        else
        {
            OnBarrelReached();
        }
    }

    private void OnFlagReached()
    {
        _builder.Build();
        _mover.UpdateStartPosition();
        _mover.StopMoving();
        _animator.SetupStaticIdle();
        _isMovingToFlag = false;
        _isMovingToBase = false;
        _targetBarrel = null;
        IsActive = false;
    }

    private void OnBarrelReached()
    {
        _mover.StopMoving();
        _grabber.PickUp(_targetBarrel);
        _animator.SetupWalkWithBarrel();
        _mover.StartMoving(_mover.StartPosition);
        _isMovingToBase = true;
        _isMovingToFlag = false;
    }

    private void OnBaseReached()
    {
        Barrel deliveredBarrel = _targetBarrel;
        _targetBarrel = null;
        deliveredBarrel.transform.SetParent(null);
        _mover.StopMoving();
        _isMovingToFlag = false;
        _isMovingToBase = false;
        _animator.SetupStaticIdle();
        BarrelWasDelivered?.Invoke(deliveredBarrel, this);
        IsActive = false;
    }

    private void SetNewBase(Base build)
    {
        _newBase = build;
        BaseWasBuilded?.Invoke(this);
    }

    private void SubscribeToNewBase()
    {
        if (_newBase.TryGetComponent(out BotStorage storage))
            storage.AddBot(this);
    }
}
