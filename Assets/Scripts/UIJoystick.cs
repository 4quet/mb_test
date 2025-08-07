using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoystick : MonoBehaviour
{
    [SerializeField]
    private GameInput _gameInput;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = transform as RectTransform;
    }

    private void Update()
    {
        if(_gameInput.IsPointerPressed && (Vector2)_rectTransform.position != _gameInput.PointerPressStartPosition)
        {
            _rectTransform.position = _gameInput.PointerPressStartPosition;
        }
    }
}
