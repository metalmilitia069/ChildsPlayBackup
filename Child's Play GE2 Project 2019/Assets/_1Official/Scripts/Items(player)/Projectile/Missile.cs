using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    /// <summary>
    /// Assign target
    /// </summary>
    /// <param name="target">GameObject's transform</param>
    public override void AssignTarget(Transform target)
    {
        _target = target;
        this.transform.LookAt(_target);
        _rigidboby.velocity = this.transform.forward * _projectileSpeed;
        transform.rotation = Quaternion.LookRotation(_rigidboby.velocity);
        Debug.DrawRay(this.transform.position, _rigidboby.velocity, Color.red, 0.5f);
    }

    /// <summary>
    /// Draw a gizmo, the range of the AOE damage.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aoeRadius);
    }
}

