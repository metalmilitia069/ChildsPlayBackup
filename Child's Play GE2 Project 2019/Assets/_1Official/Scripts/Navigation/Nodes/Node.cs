using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A place in the map where enemies move towards, this way we can control the flow of the waves as desired.  
/// </summary>
[RequireComponent(typeof(Collider))]
public class Node : MonoBehaviour
{
    /// <summary>
    /// Gets the next node from the selector.
    /// </summary>
    /// <returns> Next node, or null if this is the final node</returns>
    public Node GetNextNode()
    {
        var selector = GetComponent<AbstractNodeSelector>();
        if (selector != null)
        {
            return selector.GetNextNode();
        }
        return null;
    }

    /// <summary>
    /// When the enemy the trigger(collider), he moves to next node.
    /// </summary>
    public void OnTriggerEnter(Collider other) 
    { 
            var enemy = other.gameObject.GetComponent<EnemyMovementMechanics>();
            if (enemy != null)
            {
                enemy.GetNextNode(this);
            }
    }
}
