using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    #region Singleton
    private static HudManager _instance = null;

    public static HudManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<HudManager>();
        }
        return _instance;
    }
    #endregion
    
    [Header("HUD Components")]
    [SerializeField] private Image _foorFillImage;
    [SerializeField] private Text _foodPercentageTxt;
    [SerializeField] private Text _moneyTxt;
    [SerializeField] private Text _warmUpText;
    [SerializeField] private Text _waveInfoText;
    [SerializeField] private Text _incomingWaveText;
    [SerializeField] private Text _levelNumberText;

    /// <summary>
    /// Update the Food progress bar
    /// </summary>
    /// <param name="percentage"> The current percentage</param>
    public void UpdateFoodPercentage(float percentage)
    {
        float fill = 1.0f - percentage / 100.0f;
        if (fill < 0)
        {
            fill = 0;
        }
        _foodPercentageTxt.text = $"{fill.ToString("P0")}";
        _foorFillImage.fillAmount = fill;
    }
    
    /// <summary>
    /// Update the money amount text
    /// </summary>
    /// <param name="moneyAmount">Current amount</param>
    public void UpdateMoneyAmount(int moneyAmount)
    {
        _moneyTxt.text = $"{moneyAmount}";
    }

    /// <summary>
    /// Update the warmup text  
    /// </summary>
    /// <param name="time">Current time</param>
    public void UpdateWarmUpText(float time)
    {
        _warmUpText.text = $"Protect your food!\n" +
                           $"Ants Incoming \n" +
                           $"{time.ToString()}s";
    }
    
    /// <summary>
    /// show or hide the warmup text
    /// </summary>
    /// <param name="active">show or hide</param>
    public void ShowWarmUpText(bool active)
    {
        _warmUpText.gameObject.SetActive(active);
    }

    /// <summary>
    /// Update the Wave Info
    /// </summary>
    /// <param name="wavesLeft">how many wave lefts</param>
    /// <param name="enemiesLeft">how many enemies left in the wave</param>
    public void UpdateWaveInfoText(int wavesLeft, int enemiesLeft)
    {
        _waveInfoText.text = $"{(wavesLeft != 0 ? $"Waves Left: {wavesLeft}\n" : "LAST WAVE!\n")}" +
                             $"Enemies Left: {enemiesLeft}";
    }

    /// <summary>
    /// Update the level number text.
    /// </summary>
    /// <param name="levelNumber">level number</param>
    public void UpdateLevelNumberText(int levelNumber)
    {
        _levelNumberText.text = $"LEVEL: {levelNumber}";
    }

    /// <summary>
    /// Update the incoming wave countdown
    /// </summary>
    /// <param name="time">time till next wave</param>
    public void UpdateIncomingWaveText(float time)
    {
        if (time <= 0.0f)
        {
            _incomingWaveText.text = "";
        }
        else
        {
            _incomingWaveText.text = $"Incoming wave!\n" +
                                     $"{(int)time}s"; 
        }
    }
}
