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

    public event Action TargetReached;

    public Vector3 StartPosition => _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void StartMoving(Vector3 target)
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = StartCoroutine(MoveToTarget(target));
    }

    public void UpdateStartPosition()
    {
        _startPosition = transform.position;
    }

    public void StopMoving()
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = null;
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

        TargetReached?.Invoke();
    }
}
