using System;
using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _minimumDistance = 1f;
    private Vector3 _direction;
    private Coroutine _movingCoroutine;

    public event Action ReachedTheTarget;

    public void StartMoving(Vector3 target)
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = StartCoroutine(MoveToTarget(target));
    }

    public void StopMoving()
    {
        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = null;
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        float distance = (transform.localPosition - target).sqrMagnitude;

        while (distance > _minimumDistance)
        {
            yield return null;

            distance = (transform.localPosition - target).sqrMagnitude;
            _direction = (target - transform.position).normalized;
            transform.position += _direction * _speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        ReachedTheTarget?.Invoke();
    }
}
