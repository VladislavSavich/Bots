using System;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public event Action BaseClicked;

    private void OnMouseDown()
    {
        BaseClicked?.Invoke();
    }
}
