using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Madbox/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField, Range(1.0f, 10.0f)]
    private float _attackRange = 5.0f;
    public float AttackRange => _attackRange;

    [SerializeField, Range(0.0f, 10.0f)]
    private float _attackSpeedMultiplier = 1.0f;
    public float AttackSpeedMultiplier => _attackSpeedMultiplier;

    [SerializeField, Range(-10.0f, 10.0f)]
    private float _moveSpeedModifier = 0.0f;
    public float MoveSpeedModifier => _moveSpeedModifier;

    [SerializeField]
    private GameObject _prefab;
    public GameObject Prefab => _prefab;
}
