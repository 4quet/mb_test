using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _pointerPressInputAction;

    [SerializeField]
    private InputActionReference _pointerPositionInputAction;

    [SerializeField]
    private RectTransform _allowedInputScreenArea;

    public bool IsPointerPressed { get; private set; }
    public Vector2 PointerPressStartPosition { get; private set; }
    public Vector2 PointerPosition { get; private set; }

    private void OnEnable()
    {
        _pointerPressInputAction.action.performed += OnPointerPressPerformed;
        _pointerPressInputAction.action.canceled += OnPointerPressCanceled;
        _pointerPositionInputAction.action.performed += OnPointerPositionPerformed;
    }

    private void OnDisable()
    {
        _pointerPressInputAction.action.performed -= OnPointerPressPerformed;
        _pointerPressInputAction.action.canceled -= OnPointerPressCanceled;
        _pointerPositionInputAction.action.performed -= OnPointerPositionPerformed;
    }

    private void OnPointerPressPerformed(InputAction.CallbackContext context)
    {
        // The game's input can only be used within the allowed screen area
        // to prevent it from being triggered while interacting with UI elements
        if(RectTransformUtility.RectangleContainsScreenPoint(_allowedInputScreenArea, PointerPosition))
        {
            IsPointerPressed = true;
            PointerPressStartPosition = PointerPosition;
        }
    }

    private void OnPointerPressCanceled(InputAction.CallbackContext context)
    {
        IsPointerPressed = false;
        PointerPressStartPosition = Vector2.zero;
    }

    private void OnPointerPositionPerformed(InputAction.CallbackContext context)
    {
        PointerPosition = context.ReadValue<Vector2>();
    }
}
