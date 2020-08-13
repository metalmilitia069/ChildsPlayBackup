using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyer : Enemy
{
    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    protected override void Start()
    {
        
        SetAnimWalking();

        EnemyManager.GetInstance().AddEnemyToList(this as Enemy);
        _ogHP = HitPoints;
        
        GameObject parent = this.transform.parent.gameObject;
        this.transform.SetParent(null);
        Destroy(parent);

    }

    /// <summary>
    /// Called when the enemy attacks
    /// </summary>
    /// <param name="target">Target to attack</param>
    public override void SetAttacking(Item target)
    {
        //Nothing happens here for now, this enemy dodge barriers.
        //Might attack other stuff later.
    }

    /// <summary>
    /// Called when enemy dies.
    /// </summary>
    protected override void Die()
    {
        base.Die();

        Destroy(this.gameObject, 5);
        transform.Rotate(Vector3.up * Random.Range(-180, 180));
    }

    /// <summary>
    /// Updates the healthbar image.
    /// </summary>
    protected override void UpdateHealthBar()
    {
        // No Health bars until the displaying method is remade, using cavas was to much expensive.
    }
}
