using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Food : Player
{
    [SerializeField] private GameObject[] _pieces;

    private int _initialHP;
    private int _currentPercentage = 100;

    public int CurrentPercentage { get => _currentPercentage; private set => _currentPercentage = value; }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    new void Start()
    {
        base.Start();
        _initialHP = HitPoints;
        _pieces = GameObject.FindGameObjectsWithTag("FoodPieces");
    }

    /// <summary>
    /// Called to reduce hitPoints
    /// </summary>
    /// <param name="damageValue">Incoming Damage</param>
    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);

        _currentPercentage = 100 - (HitPoints * 100 / _initialHP);
        int qtyToRemove = (int)Mathf.Floor(_pieces.Length * (_currentPercentage / 100.0f));
        for (int i = _pieces.Length - 1; i > _pieces.Length - qtyToRemove; i--)
        {
            if (i < 0)
            {
                break;
            }
            if (_pieces[i] != null)
            {
                Destroy(_pieces[i]);
            }
        }

        HudManager.GetInstance().UpdateFoodPercentage(_currentPercentage);
        ScoreManager.GetInstance().FoodPercentage = 100 - _currentPercentage;
        ScoreManager.GetInstance().FoodEaten += damageValue;
    }

    /// <summary>
    /// Called when the hitPoints drop to zero
    /// </summary>
    protected override void Die()
    {
        GameManager.GetInstance().GameOver();
        base.Die();
    }
    
}
