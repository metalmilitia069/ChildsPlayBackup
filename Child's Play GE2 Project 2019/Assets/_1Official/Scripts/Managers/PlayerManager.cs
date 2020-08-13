using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    private static PlayerManager instance = null;

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            if (GameManager.GetInstance() != null)
            {
                instance = GameManager.GetInstance().gameObject.AddComponent<PlayerManager>(); 
            }
        }
        return instance;
    }
    #endregion

    //Variables
    private List<Player> _listOfPlayers;

    //References for Cashing
    private Player playerWithFocus;

    public List<Player> ListOfPlayers { get => _listOfPlayers; }
    public Player PlayerWithFocus { get => playerWithFocus; set => playerWithFocus = value; }


    #region Unity API Methods

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        CreatePlayerList();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public void Update()
    {
        ChangePlayerFocusWithButton();
    }
    #endregion

    #region Class Methods
    /// <summary>
    /// Populates the list of Player's Objects (Towers and Food)
    /// </summary>
    public void CreatePlayerList()
    {
        _listOfPlayers = new List<Player>();
        foreach (Player p in GameObject.FindObjectsOfType<Player>())
        {
            _listOfPlayers.Add(p);

            playerWithFocus = p;
        }        
    }

    /// <summary>
    /// Changes the Focus of a Tower or Food by pressing one Button
    /// </summary>
    private void ChangePlayerFocusWithButton()
    {
        if (Input.GetButtonDown("SwitchPlayer"))
        {
            if (_listOfPlayers.Count == 0)
            {
                return;
            }
            ClearEnemyFocusOnListAndCamera();

            int index = _listOfPlayers.IndexOf(playerWithFocus);
            index++;
            if (index >= _listOfPlayers.Count)
            {
                index = 0;
            }
            playerWithFocus = _listOfPlayers[index];
            CameraManager.GetInstance().IsLocked = true;
        }
    }
    
    /// <summary>
    /// Clear the focus on the CameraManager
    /// </summary>
    public void ClearEnemyFocusOnListAndCamera()
    {
        CameraManager.GetInstance().EnemyWithFocus = null;
    }

    /// <summary>
    /// Change the focus and tell it to the camera manager
    /// </summary>
    private void ChangePlayerFocusWithMouse()
    {
        CameraManager.GetInstance().EnemyWithFocus = null;

        int index = _listOfPlayers.IndexOf(playerWithFocus);
        index++;
        if (index >= _listOfPlayers.Count)
        {
            index = 0;
        }
        playerWithFocus = _listOfPlayers[index];
        CameraManager.GetInstance().IsLocked = true;
    }
    #endregion

    /// <summary>
    /// Add a player to _listOfPlayers
    /// </summary>
    /// <param name="_player">The player to add</param>
    public void AddPlayer(Player _player)
    {
        if (playerWithFocus == null)
        {
            playerWithFocus = _player;
        }
        _listOfPlayers.Add(_player);
    }

    /// <summary>
    /// Remove a player from _listOfPlayers
    /// </summary>
    /// <param name="_player">The player to remove</param>
    public void RemovePlayer(Player _player)
    {
        _listOfPlayers.Remove(_player);
    }
}
