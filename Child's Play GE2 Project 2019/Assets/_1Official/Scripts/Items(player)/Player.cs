using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBaseClass
{
    /// <summary>
    /// Called when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (PlayerManager.GetInstance() != null)
        {
            PlayerManager.GetInstance().RemovePlayer(this);
        }
    }
}
