using UnityEngine;

public class UIJoystick : MonoBehaviour
{
    [SerializeField]
    private GameInput _gameInput;

    [SerializeField]
    private RectTransform _innerStick;

    private RectTransform _rectTransform;
    private float _innerStickOffset;

    private void Start()
    {
        _rectTransform = transform as RectTransform;
        _innerStickOffset = _rectTransform.rect.width / 4.0f;
    }

    private void Update()
    {
        if(_gameInput.IsPointerPressed)
        {
            if((Vector2)_rectTransform.position != _gameInput.PointerPressStartPosition)
                _rectTransform.position = _gameInput.PointerPressStartPosition;

            Vector3 direction = (_gameInput.PointerPosition - _gameInput.PointerPressStartPosition).normalized;
            _innerStick.position = _rectTransform.position + direction * _innerStickOffset;
        }
        else if(_innerStick.position != _rectTransform.position)
        {
            _innerStick.position = _rectTransform.position;
        }
    }
}
