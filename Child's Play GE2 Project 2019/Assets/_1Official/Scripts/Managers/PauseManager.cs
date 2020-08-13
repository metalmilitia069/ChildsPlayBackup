using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region Singleton
    private static PauseManager _instance = null;

    public static PauseManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<PauseManager>();
        }
        return _instance;
    }
    #endregion

    /// <summary>
    /// Pause the application, set the TimeScale to 0
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        Input.ResetInputAxes();
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Unpause the application, Set the TimeScale to the value it was before pausing.
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = GameManager.GetInstance().CurrentGameSpeed;
    }

    /// <summary>
    /// Toggle the MainMenu, Pause the game and Unpause accordingly.
    /// </summary>
    public void ToggleMainMenu()
    {
        if (Time.timeScale == GameManager.GetInstance().CurrentGameSpeed)
        {
            PauseGame();
            MenuInteraction.GetInstance().PanelToggle(0);
        }
        else
        {
            UnPauseGame();
            MenuInteraction.GetInstance().PanelToggle();
        }
    }
}
