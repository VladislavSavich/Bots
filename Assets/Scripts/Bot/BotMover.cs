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

    public event Action BarrelReached;
    public event Action BaseReached;

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
        float distance = (transform.localPosition - target).sqrMagnitude;

        while (distance > _minimumDistance)
        {
            yield return null;

            distance = (transform.localPosition - target).sqrMagnitude;
            _direction = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        if (_isMovingToBase)
        {
            BaseReached?.Invoke();
        }
        else
        {
            BarrelReached?.Invoke();
        }
    }

    public void MoveToBarrel(Vector3 barrelPosition)
    {
        _isMovingToBase = false;
        StartMoving(barrelPosition);
    }

    public void MoveToBase()
    {
        _isMovingToBase = true;
        StartMoving(_startPosition);
    }

    public void StopMoving()
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = null;
    }
}
