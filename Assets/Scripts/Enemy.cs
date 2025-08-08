using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _animatorDeathTrigger = "Death";

    public float DeathTime { get; private set; }

    public void Die()
    {
        _animator.SetTrigger(_animatorDeathTrigger);
        _collider.enabled = false;
        DeathTime = Time.time;
    }

    public void ResetState()
    {
        _animator.ResetTrigger(_animatorDeathTrigger);
        _collider.enabled = true;
        DeathTime = 0.0f;
    }
}
