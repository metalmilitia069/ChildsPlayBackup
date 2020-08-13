using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    private static ScoreManager instance = null;

    public static ScoreManager GetInstance()
    {
        if (instance == null)
        {
            instance = GameManager.FindObjectOfType<ScoreManager>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _scoreDescriptionText;
    // Enemy
    private int _enemyCount;
    private int _enemyKilled;
    private int _enemyEscaped;
    public int EnemyCounts { get => _enemyCount; set => _enemyCount = value; }
    public int EnemyKilled { get => _enemyKilled; set => _enemyKilled = value; }
    public int EnemyEscaped { get => _enemyEscaped; set => _enemyEscaped = value; }

    // Food
    private float _foodPercentage;
    private int _foodEaten;
    public float FoodPercentage { get => _foodPercentage; set => _foodPercentage = value; }
    public int FoodEaten { get => _foodEaten; set => _foodEaten = value; }

    // Money
    private int _moneySpent;
    private int _moneyEarned;
    public int MoneySpent { get => _moneySpent; set => _moneySpent = value; }
    public int MoneyEarned { get => _moneyEarned; set => _moneyEarned = value; }

    // Score
    private int _score;
    public int Score { get => _score; }

    /// <summary>
    /// Reset to score and all its variable to 0.
    /// </summary>
    public void Reset()
    {
        _moneySpent = 0;
        _moneyEarned = 0;
        _foodPercentage = 100;
        _foodEaten = 0;
        _enemyCount = 0;
        _enemyKilled = 0;
        _enemyEscaped = 0;
        _score = 0;
    }

    /// <summary>
    /// Compile the score and set the textbox.
    /// </summary>
    public void CompileScore()
    {
        // Reset timescale
        GameManager.GetInstance().FastForwardButton.Init();
        Input.ResetInputAxes();
        Cursor.lockState = CursorLockMode.None;

        _score += _moneySpent;
        _score += _moneyEarned;
        _score += 10 * _enemyKilled;
        _score -= 10 * _foodEaten;
        _score -= 10 * _enemyEscaped;
        if(_score <= 0)
        {
            _score = 0;
        }
        int levelValue = (LevelManager.GetInstance().CurrentLevel+1) * 1000;
        _score += 10 * 100 * (int)FoodPercentage;
        _score += levelValue;
        _scoreDescriptionText.text = $"Money Spent:\n" +
                                    $"Money Earned:\n" +
                                    $"Enemy Killed:\n" +
                                    $"Food Eaten:\n" +
                                    $"Enemy Escaped:\n" +
                                    $"Level # bonus:\n" +
                                    $"Food Percentage Left:\n" +
                                    $"Enemy Count:\n" +
                                    $"Total Score:";

        _scoreText.text = $"(+) {_moneySpent}\n" +
                         $"(+) {_moneyEarned}\n" +
                         $"(+) 10 x {_enemyKilled}\n" +
                         $"(-) 10 x {_foodEaten}\n" +
                         $"(-) 10 x {_enemyEscaped}\n" +
                         $"(+) {levelValue}\n" +
                         $"(x) {_foodPercentage}%\n" +
                         $"{_enemyCount}\n" +
                         $"{_score}\n";
        SoundManager.GetInstance().StopMusic();
        SoundManager.GetInstance().PlaySoundOneShot(Sound.LevelCompleted);

        if (Settings.GetInstance().CheckLeaderboard(_score,LevelManager.GetInstance().CurrentLevel))
        {
            GameManager.GetInstance().PanelSelection(GameManager.GetInstance().NewRankPanelIndex);
            _scoreDescriptionText.text += $"\nNEW HIGHSCORE!";
        }
        else
        {
            GameManager.GetInstance().PanelSelection(GameManager.GetInstance().ScorePanelIndex);
            _scoreDescriptionText.text += $"\nHighScore: {Settings.GetInstance().LeaderboardScores[LevelManager.GetInstance().CurrentLevel]} by {Settings.GetInstance().LeaderboardNames[LevelManager.GetInstance().CurrentLevel]} ";
        }

    }
}
