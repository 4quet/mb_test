using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameInput _gameInput;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animatorIsMovingParameter;

    [SerializeField, Range(0.0f, 10.0f)]
    private float _moveSpeed = 5.0f;

    private void Update()
    {
        if(_gameInput.IsPointerPressed && _gameInput.PointerPosition != _gameInput.PointerPressStartPosition)
        {
            Vector2 inputDirection = (_gameInput.PointerPressStartPosition - _gameInput.PointerPosition).normalized;
            Vector3 moveDirection = new Vector3(inputDirection.x, 0.0f, inputDirection.y);

            _characterController.transform.forward = moveDirection;
            _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);

            _animator.SetBool(_animatorIsMovingParameter, true);
        }
        else if(_animator.GetBool(_animatorIsMovingParameter))
        {
            _animator.SetBool(_animatorIsMovingParameter, false);
        }
    }
}
