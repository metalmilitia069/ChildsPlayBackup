using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    private static EnemyManager _instance = null;

    public static EnemyManager GetInstance()
    {
        if (_instance == null)
        {
            if (GameManager.GetInstance() != null)
            {
                _instance = GameManager.GetInstance().gameObject.AddComponent<EnemyManager>(); 
            }
        }
        return _instance;
    }
    #endregion

    //Variables
    [SerializeField] private List<Enemy> _listOfEnemies = new List<Enemy>();

    //References for Cashing
    private Enemy _enemyWithFocus;
    public List<Enemy> ListOfEnemies { get => _listOfEnemies;}
    public Enemy EnemyWithFocus { get => _enemyWithFocus; set => _enemyWithFocus = value; }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        ChangeEnemyFocusWithButton();
    }

    /// <summary>
    /// Change the enemy the camera is focus on.
    /// </summary>
    public void ChangeEnemyFocusWithButton()
    {
        if (Input.GetButtonDown("SwitchEnemy"))
        {
            CameraManager.GetInstance().EnemyWithFocus = NextEnemyInList(CameraManager.GetInstance().EnemyWithFocus);
            CameraManager.GetInstance().IsLocked = true;
        }
    }

    /// <summary>
    /// Find the next enemy in the list
    /// </summary>
    /// <param name="enemy">enemy to find</param>
    /// <returns>return the next enemy in the list or the first one 
    ///          if you hit the last one or null if the list is empty</returns>
    public Enemy NextEnemyInList(Enemy enemy)
    {
        if (enemy == null)
        {
            if (_listOfEnemies.Count > 0)
            {
                return _listOfEnemies[0];
            }
        }
        else
        {
            int indexOfEnemy = _listOfEnemies.IndexOf(enemy);
            if (indexOfEnemy < 0)
            {
                return NextEnemyInList(null); ;
            }
            else
            {
                indexOfEnemy++;
                if (indexOfEnemy < _listOfEnemies.Count)
                {
                    return _listOfEnemies[indexOfEnemy];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Add an emeny to the list
    /// </summary>
    /// <param name="enemy">enemy to add</param>
    public void AddEnemyToList(Enemy enemy)
    {
        _listOfEnemies.Add(enemy);
        ScoreManager.GetInstance().EnemyCounts++;
    }

    /// <summary>
    /// Remove an ememy from the list
    /// </summary>
    /// <param name="enemy">enemy to remove</param>
    public void RemoveEnemyFromList(Enemy enemy)
    {
        _listOfEnemies.Remove(enemy);
        if (LevelManager.GetInstance().LevelSpawningCompleted && _listOfEnemies.Count == 0)
        {
            ScoreManager.GetInstance().CompileScore();
        }
    }

    /// <summary>
    /// Destroy all the enemy on the list.
    /// </summary>
    public void DestroyAllEnemies()
    {
        foreach (var item in GameObject.FindObjectsOfType<Enemy>())
        {
            Destroy(item.gameObject);
        }
    }
}
