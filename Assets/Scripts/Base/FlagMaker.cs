using System.Collections;
using UnityEngine;
using System;

public class FlagMaker : MonoBehaviour
{
    [SerializeField] private Rigidbody _prefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputReader _inputReader;

    private bool _isFlagCreated = false;
    private Coroutine _waitingCoroutine;
    private Rigidbody _currentFlag;

    public event Action FlagIsSet;

    public Vector3 FlagPosition => _currentFlag.transform.position;

    public bool IsFlagStand { get; private set; }

    private void Awake()
    {
        if (_camera == null)
            _camera = FindObjectOfType<Camera>();
    }

    public void StartWaitingForFlag()
    {
        StopWaitingCoroutine();
        _waitingCoroutine = StartCoroutine(WaitingFlag());
    }

    public void StopWaitingCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);
    }

    public void DestroyFlag()
    {
        if (_currentFlag != null)
        {
            _currentFlag.gameObject.SetActive(false);
            IsFlagStand = false;
        }
    }

    private void CreateFlag(Vector3 position)
    {
        _currentFlag = Instantiate(_prefab, position, Quaternion.identity);
    }

    private IEnumerator WaitingFlag()
    {
        while (enabled)
        {
            if (_inputReader.GetIsTouch()) 
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent<Base>(out _))
                        continue;

                    if (!_isFlagCreated)
                    {
                        CreateFlag(hit.point);
                        _isFlagCreated = true;
                    }
                    else
                    {
                        _currentFlag.transform.position = hit.point;
                        _currentFlag.gameObject.SetActive(true);
                    }

                    IsFlagStand = true;
                    FlagIsSet?.Invoke();
                }
            }

            yield return null;
        }
    }
}