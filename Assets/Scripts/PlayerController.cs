using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]

    [SerializeField]
    private GameInput _gameInput;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField, Range(0.0f, 10.0f)]
    private float _baseMoveSpeed = 5.0f;

    [Header("Weapons")]

    [SerializeField]
    private Transform _weaponAttachTransform;

    [SerializeField]
    private List<WeaponData> _availableWeapons;

    [Header("Animation")]

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animatorIsMovingParameter;

    // Internals
    private bool _isMoving;
    private Dictionary<WeaponData, GameObject> _weaponInstances;
    private WeaponData _equippedWeapon;

    private void Start()
    {
        _weaponInstances = new Dictionary<WeaponData, GameObject>();

        foreach(WeaponData weaponData in _availableWeapons)
        {
            GameObject weaponInstance = Instantiate(weaponData.Prefab, _weaponAttachTransform);
            weaponInstance.SetActive(false);
            _weaponInstances.Add(weaponData, weaponInstance);
        }

        EquipWeapon(_availableWeapons[0]);
    }

    private void Update()
    {
        if(_gameInput.IsPointerPressed && _gameInput.PointerPosition != _gameInput.PointerPressStartPosition)
        {
            Vector2 inputDirection = (_gameInput.PointerPressStartPosition - _gameInput.PointerPosition).normalized;
            Vector3 moveDirection = new Vector3(inputDirection.x, 0.0f, inputDirection.y);

            _characterController.transform.forward = moveDirection;
            _characterController.Move(moveDirection * GetMoveSpeed() * Time.deltaTime);

            _isMoving = true;
            _animator.SetBool(_animatorIsMovingParameter, true);
            _weaponAttachTransform.gameObject.SetActive(false);
        }
        else if(_isMoving)
        {
            _isMoving = false;
            _animator.SetBool(_animatorIsMovingParameter, false);
            _weaponAttachTransform.gameObject.SetActive(true);
        }
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if(_equippedWeapon != null)
        {
            _weaponInstances[_equippedWeapon].SetActive(false);
        }
        if(_weaponInstances.TryGetValue(weaponData, out GameObject weaponInstance))
        {
            _equippedWeapon = weaponData;
            weaponInstance.SetActive(true);
        }
    }

    public float GetMoveSpeed()
    {
        return _baseMoveSpeed + (_equippedWeapon != null ? _equippedWeapon.MoveSpeedModifier : 0.0f);
    }
}
