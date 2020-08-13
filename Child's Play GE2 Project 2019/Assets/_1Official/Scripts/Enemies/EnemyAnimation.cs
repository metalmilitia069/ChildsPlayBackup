using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField,Range(0.21f,2.0f)] private float _animSpeedRandom = 1;
    private const float RANDSPEEDBOUND = 0.2f;

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        _animator.SetFloat("Speed", Random.Range(_animSpeedRandom - RANDSPEEDBOUND, _animSpeedRandom + RANDSPEEDBOUND));
    }

    /// <summary>
    /// Change animation to walking
    /// </summary>
    public void SetWalking()
    {
        _animator.SetTrigger("Walking");
    }

    /// <summary>
    /// Change animation to attacking
    /// </summary>
    public void SetAttacking()
    {
        _animator.SetTrigger("Attacking");
    }

    /// <summary>
    /// Change animation to Dizzy
    /// </summary>
    public void SetDizzy()
    {
        _animator.SetTrigger("Dizzy");
    }

    /// <summary>
    /// Change animation to retreating
    /// </summary>
    public void SetRetreating()
    {
        _animator.SetTrigger("Retreating");
    }

}
