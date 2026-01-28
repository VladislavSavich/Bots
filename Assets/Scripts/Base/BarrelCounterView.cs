using TMPro;
using UnityEngine;

public class BarrelCounterView : MonoBehaviour
{
    [SerializeField] private BarrelCounter _counter;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private float heightOffset;
    [SerializeField] private float widthOffset;

    private void Start()
    {
        _score.transform.position = new Vector3(_counter.transform.position.x + widthOffset, _counter.transform.position.y + heightOffset, _counter.transform.position.z);
    }

    private void OnEnable()
    {
        _counter.CountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        _counter.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        if (_score != null)
        {
            _score.text = count.ToString();
        }
    }
}
