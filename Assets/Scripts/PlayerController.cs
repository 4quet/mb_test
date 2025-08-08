using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        None = 0,
        Idle = 1,
        Moving = 2,
        Attacking = 3,
    }

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
    private LayerMask _enemyLayerMask;

    [SerializeField]
    private List<WeaponData> _availableWeapons;

    [Header("Animation")]

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animatorStateParameter;

    // Internals
    private PlayerState _state;
    private Dictionary<WeaponData, GameObject> _weaponInstances;
    private WeaponData _equippedWeapon;
    private Enemy _closestEnemy;

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

            SetState(PlayerState.Moving);
        }
        else
        {
            if(IsAnyEnemyInRange(out _closestEnemy))
            {
                _characterController.transform.LookAt(_closestEnemy.transform);
                SetState(PlayerState.Attacking);
            }
            else
            {
                SetState(PlayerState.Idle);
            }
        }
    }

    private void SetState(PlayerState state)
    {
        if(_state != state)
        {
            _state = state;
            _animator.SetInteger(_animatorStateParameter, (int)_state);
            _weaponAttachTransform.gameObject.SetActive(_state != PlayerState.Moving);
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

    private bool IsAnyEnemyInRange(out Enemy closestEnemy)
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _equippedWeapon.AttackRange, _enemyLayerMask);
        Collider closestEnemyCollider = null;
        float closestDistance = _equippedWeapon.AttackRange + 1.0f;

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            Collider enemy = enemyColliders[i];
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemyCollider = enemy;
            }
        }

        if(closestEnemyCollider != null)
        {
            closestEnemy = closestEnemyCollider.GetComponentInChildren<Enemy>();
            Debug.Assert(closestEnemy != null, "Closest enemy should not be null " + closestEnemyCollider.name + closestEnemyCollider.gameObject.GetInstanceID());
            return true;
        }

        closestEnemy = null;
        return false;
    }

    private void OnAnimationAttackDamage(int id)
    {
        if(_closestEnemy != null)
        {
            _closestEnemy.Die();
        }
    }

    public float GetMoveSpeed()
    {
        return _baseMoveSpeed + (_equippedWeapon != null ? _equippedWeapon.MoveSpeedModifier : 0.0f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(_equippedWeapon != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _equippedWeapon.AttackRange);
        }
    }
#endif
}
