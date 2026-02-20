using System.Collections;
using UnityEngine;
using System;

public class FlagMaker : MonoBehaviour
{
    [SerializeField] private GameObject _flagPrefab;
    [SerializeField] private Camera _camera;

    private Coroutine _waitingCoroutine;
    private GameObject _currentFlag;

    public Vector3 FlagPosition => _currentFlag.transform.position;


    public event Action FlagIsSet;

    public bool IsFlagStand { get; private set; }

    private void Awake()
    {
        if (_camera == null)
            _camera = FindObjectOfType<Camera>();
    }

    private IEnumerator WaitingFlag()
    {
        while (enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentFlag != null)
                    DestroyFlag();

                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (!hit.collider.TryGetComponent<Base>(out _))
                    {
                        CreateFlag(hit.point);
                        IsFlagStand = true;
                        FlagIsSet?.Invoke();
                    }
                }
            }

            yield return null;
        }
    }

    private void CreateFlag(Vector3 position)
    {
        _currentFlag = Instantiate(_flagPrefab, position, Quaternion.identity);
    }

    private void StopWaitingCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);
    }

    public void DestroyFlag()
    {
        Destroy(_currentFlag);
        IsFlagStand = false;
        _currentFlag = null;
    }

    public void StartWaitingForFlag()
    {
        StopWaitingCoroutine();

        _waitingCoroutine = StartCoroutine(WaitingFlag());
    }
}
