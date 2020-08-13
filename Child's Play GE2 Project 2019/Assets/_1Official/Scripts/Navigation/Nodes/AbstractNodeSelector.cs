using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract class meant as base for allowing enemies to select next position to move towards.
/// </summary>  
public abstract class AbstractNodeSelector : MonoBehaviour
{
    /// <summary>
    /// Here is where enemies look for the posible nodes to navigate towards.
    /// </summary>
    public List<Node> listOfNodes;

    /// <summary>
    /// Gets the next node in listOfNodes
    /// </summary>
    /// <returns>The next node in the list of Nodes, null if the node is the endpoint</returns>
    public abstract Node GetNextNode();
}

