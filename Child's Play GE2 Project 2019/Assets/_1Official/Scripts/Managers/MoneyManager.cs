using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    #region Singleton
    private static MoneyManager _instance = null;

    public static MoneyManager GetInstance()
    {
        if (_instance == null)
        {
            if (GameManager.GetInstance() != null)
            {
                _instance = GameManager.GetInstance().gameObject.AddComponent<MoneyManager>(); 
            }
        }
        return _instance;
    }
    #endregion
    
    private int _currentMoney;
    public int CurrentMoney { get => _currentMoney; }

    /// <summary>
    /// Reset the money to 0 or to the incomming value
    /// </summary>
    /// <param name="setMoney">Value to set the money at.</param>
    public void ResetMoney(int setMoney = 0)
    {
        _currentMoney = setMoney;
        UpdateMoneyText();
    }

    /// <summary>
    /// Change the money amount, positive or negative
    /// </summary>
    /// <param name="change">The amount to change by</param>
    public void MoneyChange(int change)
    {
        if (change < 0)
        {
            ScoreManager.GetInstance().MoneySpent += Mathf.Abs(change);
        }
        _currentMoney += change;
        UpdateMoneyText();
    }

    /// <summary>
    /// Update the hud.
    /// </summary>
    public void UpdateMoneyText()
    {
        HudManager.GetInstance().UpdateMoneyAmount(_currentMoney);
    }

    /// <summary>
    /// Check if the player has enough money to by said item.
    /// </summary>
    /// <param name="cost">the cost of the item</param>
    /// <returns>True if enough money is available</returns>
    public bool TryToBuy(int cost)
    {
        if (_currentMoney >= cost)
        {
            MoneyChange(-cost);
            return true;
        }

        Debug.Log("Not Enough Cash.");
        return false;
    }

}