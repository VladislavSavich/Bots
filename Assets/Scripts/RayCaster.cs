using System;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private InputReader _inputReader;

    private Ray _ray;

    private void Update()
    {
        if (_inputReader.GetIsTouch())
            Hit();
    }

    private void Hit()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out Base building))
                building.StartColonization();
        }
    }
}
