using UnityEngine;

public class InputReader : MonoBehaviour
{
    private bool _isTouch;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _isTouch = true;
    }

    public bool GetIsTouch() => GetBoolAsTrigger(ref _isTouch);

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }
}
