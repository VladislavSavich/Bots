using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Ray _ray;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Hit();
        }
    }

    private void Hit()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent<Base>(out Base building))
                building.StartColonization();
        }
    }
}
