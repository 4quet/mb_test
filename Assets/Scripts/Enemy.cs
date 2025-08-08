using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animatorDeathTrigger = "Death";

    public void Die()
    {
        _collider.enabled = false;
        _animator.SetTrigger(_animatorDeathTrigger);
    }
}
