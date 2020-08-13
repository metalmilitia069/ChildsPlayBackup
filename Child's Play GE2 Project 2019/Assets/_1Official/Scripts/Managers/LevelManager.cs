using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Level
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private int _initialMoney;
    [Header("Set item available, Including upgrades")]
    [SerializeField] private Item[] _itemsAvailable;

    public GameObject LevelPrefab { get => _levelPrefab; }
    public int InitialMoney { get => _initialMoney; }
    public string Name { get => _name; }
    public Item[] ItemsAvailable { get => _itemsAvailable; }
}

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager _instance = null;

    public static LevelManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<LevelManager>();
        }
        return _instance;
    }
    #endregion

    [SerializeField] Level[] _levels;
    private GameObject _currentLevelGO;
    private int _currentLevel = 0;
    private bool _currLvlCompleted = false;
    private Transform _root;
    private bool _levelSpawningCompleted = false;

    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public bool LevelSpawningCompleted { get => _levelSpawningCompleted; }
    public GameObject CurrentLevelGO { get => _currentLevelGO; }
    public Level CurrentLevelInfo { get => _levels[_currentLevel]; }
    

    void Start()
    {
        _root = GameObject.FindGameObjectWithTag("Root").transform;
        _currentLevel = Settings.GetInstance().StartingLevel;
        LoadLevel(_currentLevel);
    }

    /// <summary>
    /// Load the next level in line
    /// </summary>
    public void LoadNextLevel()
    {
        LoadLevel(++_currentLevel);
    }

    /// <summary>
    /// Load the level as per index
    /// </summary>
    /// <param name="levelNumber">Level to load</param>
    public void LoadLevel(int levelNumber = 0)
    {
        if (levelNumber >= _levels.Length)
        {
            GameCompleted();
            return;
        }
        UpdateSettings();

        ScoreManager.GetInstance().Reset();
        if (_currentLevelGO != null)
        {
            DestroyImmediate(_currentLevelGO);
        }

        _currentLevelGO = Instantiate(_levels[levelNumber].LevelPrefab, _root.position, _root.rotation, null);
        SoundManager.GetInstance().PlayMusic(_currentLevel);
        MoneyManager.GetInstance().ResetMoney(_levels[levelNumber].InitialMoney);
        EnemyManager.GetInstance().DestroyAllEnemies();
        _levelSpawningCompleted = false;

        GameManager.GetInstance().FastForwardButton.Init();
        GameManager.GetInstance().DeselectTile();

        PlayerManager.GetInstance().CreatePlayerList();
        CameraManager.GetInstance().CameraLockerButton(false);
        HudManager.GetInstance().UpdateLevelNumberText(levelNumber + 1); // plus one since level are base on indexes
    }


    /// <summary>
    /// Set the Level to completed mode, reference used by enemy manager 
    /// to call score compile when last enemy dies after.
    /// </summary>
    public void LevelCompleted()
    {
        _levelSpawningCompleted = true;
        
    }

    /// <summary>
    /// Call the wining panel, when last level is completed.
    /// </summary>
    public void GameCompleted()
    {
        GameManager.GetInstance().PanelSelection(GameManager.GetInstance().WinPanelIndex);
        SoundManager.GetInstance().StopMusic();
        SoundManager.GetInstance().PlaySoundOneShot(Sound.WinCompleted);
    }

    /// <summary>
    /// Call to update Settings.
    /// This will retreive the level to load when loading the scene.
    /// </summary>
    public void UpdateSettings()
    {
        if (Settings.GetInstance().LevelsUnlocked < _currentLevel)
        {
            Settings.GetInstance().LevelsUnlocked = _currentLevel;
            Settings.GetInstance().SaveLevelParams();
        }
    }
}
