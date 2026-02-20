using TMPro;
using UnityEngine;

public class BarrelCounterView : MonoBehaviour
{
    [SerializeField] private BarrelCounter _counter;
    [SerializeField] private float heightOffset;
    [SerializeField] private float widthOffset;

    private float _textScale = 0.3f;
    private GameObject _textObject;
    private TextMeshPro _score;

    private void Awake()
    {
        _textObject = new GameObject();
        _score = _textObject.AddComponent<TextMeshPro>();
    }

    private void Start()
    {
        _score.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _score.transform.localScale = new Vector3(_textScale, _textScale, _textScale);
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
