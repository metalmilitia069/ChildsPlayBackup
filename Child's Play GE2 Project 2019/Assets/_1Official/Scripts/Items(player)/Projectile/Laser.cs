using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Laser : Projectile
{
    [SerializeField] private float _secondPerTick;
    [SerializeField] private float _secondItLast;

    /// <summary>
    /// Damage the oject it touch.
    /// </summary>
    /// <param name="enemyGO"></param>
    public override void Damage(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.DamageOverTime(_damageValue, _secondPerTick, _secondItLast);
        }
    }

}
