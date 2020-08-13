using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class blockBehaviour : MonoBehaviour
{
    private List<EnemyMovementMechanics> attackers;
    private float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        attackers = new List<EnemyMovementMechanics>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("My health is: " + health);
        if (health <= 0)
        {
            Wallbreached();
        }
    }

    /// <summary>
    /// Add all new enemies on a list to keep track of the attackers.
    /// <summary>
    private void AddAttacker(Collider enemy)
    {
        var _enemy = enemy.gameObject.GetComponent<EnemyMovementMechanics>();
        _enemy.AttackStance(this.transform.position);
        attackers.Add(_enemy);
    }

    /// <summary>
    /// Enemies Will continue to move towards it's objective.
    /// </summary>
    private void LetGotEnemy()
    {
        foreach (var enemy in attackers)
        {
            enemy.MoveOnStance();
        }
    }

    /// <summary>
    /// Destroys the barrier. 
    /// </summary>
    public void Wallbreached()
    { 
        LetGotEnemy();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Reduces health based on the enemy damage value.
    /// </summary>
    private void UnderAttack(float damage)
    {
        health -= damage* Time.deltaTime;
    }

    #region Unity trigger events

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AddAttacker(other);
        } 
    }
      
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var _enemy = other.gameObject.GetComponent<EnemyMovementMechanics>();
            //UnderAttack(_enemy.AttackDamage());
        }
    }

    #endregion

}
