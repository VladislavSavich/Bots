using System;
using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _minimumDistance = 1f;
    private Vector3 _direction;
    private Vector3 _startPosition;
    private Coroutine _movingCoroutine;
    private bool _isMovingToBase;
    private bool _isMovingToFlag;

    public event Action BarrelReached;
    public event Action BaseReached;
    public event Action FlagReached;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void StartMoving(Vector3 target)
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = StartCoroutine(MoveToTarget(target));
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        float distance = (transform.position - target).sqrMagnitude;

        while (distance > _minimumDistance)
        {
            yield return null;

            distance = (transform.position - target).sqrMagnitude;
            _direction = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        if (_isMovingToBase)
        {
            BaseReached?.Invoke();
        }
        else if (_isMovingToFlag)
        {
            FlagReached?.Invoke();
        }
        else
        {
            BarrelReached?.Invoke();
        }
    }

    public void UpdateStartPosition()
    {
        _startPosition = transform.position;
    }

    public void MoveToBarrel(Vector3 barrelPosition)
    {
        _isMovingToFlag = false;
        _isMovingToBase = false;
        StartMoving(barrelPosition);
    }

    public void MoveToBase()
    {
        _isMovingToFlag = false;
        _isMovingToBase = true;
        StartMoving(_startPosition);
    }

    public void MoveToFlag(Vector3 flagPosition)
    {
        _isMovingToBase = false;
        _isMovingToFlag = true;
        StartMoving(flagPosition);
    }

    public void StopMoving()
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = null;
        _isMovingToBase = false;
        _isMovingToFlag = false;
    }
}
