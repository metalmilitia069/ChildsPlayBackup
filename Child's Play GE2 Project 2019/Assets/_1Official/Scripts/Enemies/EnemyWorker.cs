using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorker : Enemy
{
    /// <summary>
    /// Called before the first frame update
    /// </summary>
    protected override void Start()
    {
        SetAnimWalking();

        EnemyManager.GetInstance().AddEnemyToList(this as Enemy);
        _ogHP = HitPoints;
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
