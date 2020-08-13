#if UNITY_EDITOR
//using UnityEngine;
#endif


/// <summary>
/// Selects the next node in the order it appears on the list.
/// </summary>
public class DeterminedNodeSelector : AbstractNodeSelector
{
    /// <summary>
    /// Index to keep track of what node should be selected next
    /// </summary>
    protected int nextNode;

    /// <summary>
    /// Selects the next node to travel to.
    /// </summary>
    /// <returns>The next selected node, or null if there are no valid nodes</returns>
    public override Node GetNextNode()
    {
        if (listOfNodes.Next(ref nextNode, true))
        {
            return listOfNodes[nextNode];
        }
        return null;
    }
}


