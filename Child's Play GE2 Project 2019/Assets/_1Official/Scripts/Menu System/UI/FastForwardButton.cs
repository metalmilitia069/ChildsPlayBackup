using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForwardButton : MonoBehaviour
{
    [SerializeField] Image[] _arrows;
    [SerializeField] Image _skipArrow;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Start()
    {
        foreach (Image arrow in _arrows)
        {
            arrow.enabled = false;
        }
    }

    /// <summary>
    /// Change the TimeScale of the game and display or hides the arrows accordingly
    /// </summary>
    public void ChangeSpeed()
    {
        if (SpawnManager.GetInstance().WarmupCounter <= 0)
        {

            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = GameManager.GetInstance().CurrentGameSpeed = GameManager.GetInstance().SpeedMulOne;
                for (int i = 0; i < 2; i++)
                {
                    _arrows[i].enabled = true;
                }
                //sound
            }
            else if (Time.timeScale == GameManager.GetInstance().SpeedMulOne)
            {
                Time.timeScale = GameManager.GetInstance().CurrentGameSpeed = GameManager.GetInstance().SpeedMulTwo;
                for (int i = 2; i < 3; i++)
                {
                    _arrows[i].enabled = true;
                }
                //sound
            }
            else
            {
                Time.timeScale = GameManager.GetInstance().CurrentGameSpeed = 1.0f;
                for (int i = 1; i < 3; i++)
                {
                    _arrows[i].enabled = false;
                }
                //sound
            } 
        }
        else
        {
            //Insert the SpawnManager Method to skip warmup
            SpawnManager.GetInstance().SkipWarmUp();
            //skipArrow.enabled = false;
        }
    }

    /// <summary>
    /// Initialize the TimeScale and call the skip warmup coroutine.
    /// </summary>
    public void Init()
    {
        Time.timeScale = GameManager.GetInstance().CurrentGameSpeed = 1.0f;
        StartCoroutine(SkipWarmup());
    }

    /// <summary>
    /// Display the big arrow while the warmup is happening. 
    /// Exit and set the TimeScale to '1' onces the warmup counter reach 0
    /// </summary>
    private IEnumerator SkipWarmup()
    {
        foreach (Image arrow in _arrows)
        {
            arrow.enabled = false;
        }
        _skipArrow.enabled = true;
        while (SpawnManager.GetInstance().WarmupCounter > 0)
        {
            yield return null;
        }
        _skipArrow.enabled = false;
        for (int i = 0; i < 1; i++)
        {
            Time.timeScale = GameManager.GetInstance().CurrentGameSpeed = 1.0f;
            _arrows[i].enabled = true;
        }
    }
}
