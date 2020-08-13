

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    
    #region Singleton
    private static Settings instance = null;

    public static Settings GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<Settings>();
        }
        return instance;
    }
    #endregion

    [Header("Settings")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerSnapshot _inMenuSS;
    public AudioMixerSnapshot InMenuSS { get { return _inMenuSS; } }
    [SerializeField] private AudioMixerSnapshot _inNormalSS;
    public AudioMixerSnapshot InNormalSS { get { return _inNormalSS; } }
    [SerializeField] private AudioMixerSnapshot _inWaterSS;
    public AudioMixerSnapshot InWaterSS { get { return _inWaterSS; } }
    [SerializeField] private string _masterVolParam;
    public string MasterVolParam { get { return _masterVolParam; } }
    [SerializeField] private string _musicVolParam;
    public string MusicVolParam { get { return _musicVolParam; } }
    [SerializeField] private string _sFXVolParam;
    public string SFXVolParam { get { return _sFXVolParam; } }
    [SerializeField] private string _sensitivityHParam;
    public string SensitivityHParam { get { return _sensitivityHParam; } }
    [SerializeField] private string _sensitivityVParam;
    public string SensitivityVParam { get { return _sensitivityVParam; } }

    [Header("Setting Panel Interactables")]
    [SerializeField] private SetVolume _masterVol;
    [SerializeField] private SetVolume _musicVol;
    [SerializeField] private SetVolume _sfxVol;
    [SerializeField] private SetGFX _qualitySetting;
    [SerializeField] private SetSensitivity _sensitivityHLevel;
    [SerializeField] private SetSensitivity _sensitivityVLevel;
    [SerializeField] private Button _applyButton;
    public Button ApplyButton { get { return _applyButton; } }

    [Header("Leaderboard")]
    [SerializeField] private Text _rankedText;
    [SerializeField] private Text _lbScoreText;
    [SerializeField] private Text _lbNameText;
    private List<int> _leaderboardScores;
    public List<int> LeaderboardScores { get => _leaderboardScores; set { _leaderboardScores = value; } }
    private List<string> _leaderboardNames;
    public List<string> LeaderboardNames { get => _leaderboardNames; set { _leaderboardNames = value; } }
    private int _score;
    private int _currentLevel;
    public int CurrentLevel { get => _currentLevel; set { _currentLevel = value; } }

    private float _masterVolValue;
    public float MasterVolValue { get => _masterVolValue; set { _masterVolValue = value; } }
    private float _musicVolValue;
    public float MusicVolValue { get => _musicVolValue; set { _musicVolValue = value; } }
    private float _sFXVolValue;
    public float SFXVolValue { get { return _sFXVolValue; } set { _sFXVolValue = value; } }
    private float _sensitivityH;
    public float SensitivityH { get { return _sensitivityH; } set { _sensitivityH = value; } }
    private float _sensitivityV;
    public float SensitivityV { get { return _sensitivityV; } set { _sensitivityV = value; } }

    public int StartingLevel { get => _startingLevel; set => _startingLevel = value; }
    public int LevelsUnlocked { get => _levelsUnlocked; set => _levelsUnlocked = value; }

    [Header("Game Specific")]
    [SerializeField] private int _startingLevel = 0;
    [SerializeField] private string _startingLevelParam;
    [SerializeField] private int _levelsUnlocked = 0;
    [SerializeField] private string _levelsUnlockedParam;
    [SerializeField] private int _MaxLevel = 5;

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        // Load settings
        _masterVolValue = PlayerPrefs.GetFloat(_masterVolParam, -10);
        _audioMixer.SetFloat(_masterVolParam, _masterVolValue);
        _musicVolValue = PlayerPrefs.GetFloat(_musicVolParam, -10);
        _audioMixer.SetFloat(_musicVolParam, _musicVolValue);
        _sFXVolValue = PlayerPrefs.GetFloat(_sFXVolParam, -10);
        _audioMixer.SetFloat(_sFXVolParam, _sFXVolValue);

        _qualitySetting.PrefValue = QualitySettings.GetQualityLevel();
        _sensitivityH = PlayerPrefs.GetFloat(_sensitivityHParam, 25);
        _sensitivityV = PlayerPrefs.GetFloat(_sensitivityVParam, 25);
        GetLeaderboard();

        LoadLevelParams();
    }

    /// <summary>
    /// Save the player pref to the disk.
    /// </summary>
    public void SaveChanges()
    {
        PlayerPrefs.SetFloat(_masterVolParam, _masterVol.Slider.value);
        PlayerPrefs.SetFloat(_musicVolParam, _musicVol.Slider.value);
        PlayerPrefs.SetFloat(_sFXVolParam,   _sfxVol.Slider.value);
        int gfxIndex = (int)Mathf.Floor(_qualitySetting.Slider.value);
        QualitySettings.SetQualityLevel(gfxIndex, true);
        PlayerPrefs.SetFloat(_sensitivityHLevel.NameParam, _sensitivityHLevel.Slider.value);
        _sensitivityH = _sensitivityHLevel.Slider.value;
        PlayerPrefs.SetFloat(_sensitivityVLevel.NameParam, _sensitivityVLevel.Slider.value);
        _sensitivityV = _sensitivityVLevel.Slider.value;
        PlayerPrefs.Save();
        _applyButton.interactable = false;
    }

    /// <summary>
    /// Cancel the change made by the user on the setting panel.
    /// </summary>
    public void CancelChanges()
    {
        float masterVolValue = PlayerPrefs.GetFloat(_masterVolParam, 0);
        _masterVol.SetVol(masterVolValue);

        float musicVolValue = PlayerPrefs.GetFloat(_musicVolParam, 0);
        _musicVol.SetVol(musicVolValue);

        float sfxVolValue = PlayerPrefs.GetFloat(_sFXVolParam, 0);
        _sfxVol.SetVol(sfxVolValue);

        _qualitySetting.SetQuality(_qualitySetting.PrefValue);

        float sensitivityHValue = PlayerPrefs.GetFloat(_sensitivityHLevel.NameParam, 2);
        _sensitivityHLevel.SetSens(sensitivityHValue);
        float sensitivityVValue = PlayerPrefs.GetFloat(_sensitivityVLevel.NameParam, 2);
        _sensitivityVLevel.SetSens(sensitivityVValue);

        _applyButton.interactable = false;

    }

    /// <summary>
    /// Reset the all settings to the default values
    /// </summary>
    public void ResetSettings()
    {
        if (_masterVol.Slider.value != -10 || _musicVol.Slider.value != -10
            || _sfxVol.Slider.value != -10 || _qualitySetting.Slider.value != 3
            || _sensitivityHLevel.Slider.value != 2)
        {
            _masterVol.SetVol(-10);
            _musicVol.SetVol(-10);
            _sfxVol.SetVol(-10);
            _qualitySetting.SetQuality(3);
            _sensitivityHLevel.SetSens(25);
            _sensitivityVLevel.SetSens(25);
        }
    }

    /// <summary>
    /// Get the leaderboard saved on disk.
    /// </summary>
    public void GetLeaderboard()
    {
        _lbScoreText.text = string.Empty;
        _lbNameText.text = string.Empty;
        _leaderboardScores = new List<int>();
        for (int i = 0; i < _MaxLevel; i++)
        {
            _leaderboardScores.Add(PlayerPrefs.GetInt("Score" + i, 0));
        }
        _leaderboardNames = new List<string>();
        for (int i = 0; i < _MaxLevel; i++)
        {
            _leaderboardNames.Add(PlayerPrefs.GetString("Name" + i, "AAA").ToUpper());
        }
        for (int i = 0; i < _MaxLevel; i++)
        {
            if (i != 0)
            {
                _lbScoreText.text += "\n" + _leaderboardScores[i].ToString("D8");
                _lbNameText.text += "\n- " + _leaderboardNames[i];
            }
            else
            {
                _lbScoreText.text += _leaderboardScores[i].ToString("D8");
                _lbNameText.text += "- " + _leaderboardNames[i];
            }
        }

    }

    /// <summary>
    /// Check if the score is higher than what the leaderboard says based on the level
    /// <para>Condition : current score > current high score </para>
    /// </summary>
    /// <param name="score">score to check</param>
    /// <param name="currentLevel">level to verify</param>
    /// <returns>return true if current score is higher than current highscore</returns>
    public bool CheckLeaderboard(int score, int currentLevel)
    {
        this._score = score;
        this._currentLevel = currentLevel;
        return score > _leaderboardScores[currentLevel];
    }

    /// <summary>
    /// Set the leaderboard and save it to disk
    /// </summary>
    /// <param name="playerName">The name of the player</param>
    public void SetLoaderboard(string playerName = "NEW")
    {
        _rankedText.text = "Newest Highscore on level " + (_currentLevel + 1).ToString() + ".";
        _leaderboardScores[_currentLevel] = _score;
        if (_currentLevel < _MaxLevel)
        {
            _leaderboardNames[_currentLevel] = playerName;
        }
        for (int i = 0; i < _MaxLevel; i++)
        {
            PlayerPrefs.SetInt("Score" + i, _leaderboardScores[i]);
        }
        for (int i = 0; i < _MaxLevel; i++)
        {
            PlayerPrefs.SetString("Name" + i, _leaderboardNames[i].ToUpper());
        }
        PlayerPrefs.Save();
        GetLeaderboard();
    }

    /// <summary>
    /// Load the level information.
    /// The starting level and how many level are unlocked.
    /// </summary>
    public void LoadLevelParams()
    {
        _levelsUnlocked = PlayerPrefs.GetInt(_levelsUnlockedParam, 0);
        _startingLevel = PlayerPrefs.GetInt(_startingLevelParam, 0);
    }

    /// <summary>
    /// Save the level progression to disk. 
    /// </summary>
    public void SaveLevelParams()
    {
        PlayerPrefs.SetInt(_levelsUnlockedParam, _levelsUnlocked);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reset the level parameter
    /// </summary>
    public void ResetLevelParams()
    {
        PlayerPrefs.SetInt(_levelsUnlockedParam, 0);
        _levelsUnlocked = 0;
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Unlock all the level for the player
    /// </summary>
    public void UnlockAllLevelParams()
    {
        _levelsUnlocked = _MaxLevel;
        PlayerPrefs.SetInt(_levelsUnlockedParam, _levelsUnlocked);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reset the progression the player has.
    /// </summary>
    public void ResetUnlockedProgression()
    {
        _levelsUnlocked = 0;
        PlayerPrefs.SetInt(_levelsUnlockedParam, _levelsUnlocked);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set the starting level when the scene is loaded
    /// </summary>
    /// <param name="levelNumber">The level to set as starting point</param>
    public void SetStartingLevel(int levelNumber = 0)
    {
        _startingLevel = levelNumber - 1; // Index is 0, so -1 is important
        PlayerPrefs.SetInt(_startingLevelParam, _startingLevel);
    }
}
