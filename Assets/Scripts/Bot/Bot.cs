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

    public event Action<Barrel, Bot> BarrelWasDelivered;
    public event Action<Bot> BaseWasBuilded;

    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        _mover.BarrelReached += OnBarrelReached;
        _mover.BaseReached += OnBaseReached;
        _mover.FlagReached += OnFlagReached;
        _builder.BaseBuilded += SetNewBase;
    }

    private void OnDisable()
    {
        _mover.BarrelReached -= OnBarrelReached;
        _mover.BaseReached -= OnBaseReached;
        _mover.FlagReached -= OnFlagReached;
        _builder.BaseBuilded -= SetNewBase;
    }

    private void OnFlagReached()
    {
        _builder.Build();
        _mover.UpdateStartPosition();
        _mover.StopMoving();
        _animator.SetupStaticIdle();
        IsActive = false;
        _targetBarrel = null;
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

    private void SetNewBase(Base build)
    {
        _newBase = build;
        BaseWasBuilded?.Invoke(this);
    }

    private IEnumerator DelayedSubscribe()
    {
        yield return _subscribeDelay;

        if (_newBase != null)
        {
            _newBase.SubscribeToBot(this);
        }
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

    public void SetFlagPosition(Vector3 position)
    {
        IsActive = true;
        _mover.MoveToFlag(position);
        _animator.SetupWalk();
    }

    public void SubscribeToNewBase()
    {
        StartCoroutine(DelayedSubscribe());
    }
}
