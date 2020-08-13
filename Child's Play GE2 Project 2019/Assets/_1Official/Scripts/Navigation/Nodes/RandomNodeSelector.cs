using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RandomNodeSelector : AbstractNodeSelector
{
    public override Node GetNextNode()
    {
        if (listOfNodes == null)
        {
            return null;
        }
        //float chance = Random.value;
        if (Random.value > 0.5)
        {
            return listOfNodes[0];
        }
        return listOfNodes[1];
    }
}



