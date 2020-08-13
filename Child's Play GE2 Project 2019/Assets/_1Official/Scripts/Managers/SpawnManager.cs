using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    private static SpawnManager _instance = null;

    public static SpawnManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<SpawnManager>();
        }
        return _instance;
    }
    #endregion

    [Header("Waves setup for the level.")]
    [SerializeField] private Node _spawnPoint;
    [SerializeField] private float _warmUpSeconds = 10.0f;
    [SerializeField] private float _timeBetweenWaves = 5.0f;
    [SerializeField] private WaveSetup_SO[] _wavesSetup;
    private float _warmuUpCounter;
    private int _enemyLeftToSpawn = 0;
    private int _currentWave = 1;
    private int _wavesLeft = 0;

    private float _counterToNextWave;
    private int _waveIndex = 0;
    private int _subWaveIndex = 0;
    private int _waveCounter = 0;



    private bool _warmedUp = false;
    private bool _startNewWave = false;
    private bool _startCounter = false;

    public Node SpawnPoint { get => _spawnPoint; }
    public float WarmupCounter { get => (int)(_warmuUpCounter - Time.time); }
   
    /// <summary>
    /// Called as soon as the GameObject is instanticated
    /// </summary>
    void Awake()
    {   
        _warmuUpCounter = Time.time + _warmUpSeconds;
        HudManager.GetInstance().ShowWarmUpText(true);
        _wavesLeft = _wavesSetup.Length;
        GetEnemiesLeftToSpawn();
        HudManager.GetInstance().UpdateWaveInfoText(_wavesLeft, _enemyLeftToSpawn);
        HudManager.GetInstance().UpdateIncomingWaveText(0.0f);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        WarmingUp();
        StartWave();
        WaveCounter();
    }

    /// <summary>
    /// Check if its time to start a new wave.
    /// </summary>
    private void WaveCounter()
    {
        if (_startCounter)
        {
            _counterToNextWave -= Time.deltaTime;
            if (_counterToNextWave <= 0)
            {
                _startNewWave = true;
                _startCounter = false;
            }
            _counterToNextWave = Mathf.Clamp(_counterToNextWave, 0f, Mathf.Infinity);
            HudManager.GetInstance().UpdateIncomingWaveText(_counterToNextWave);
        }
    }

    /// <summary>
    /// Initialize the Wave coroutine
    /// </summary>
    private void StartWave()
    {
        if (_startNewWave)
        {
            StartCoroutine(WaveSpawner());
            _startNewWave = false;
        }
    }

    /// <summary>
    /// Check if the level is in warmup mode and update the hud
    /// </summary>
    private void WarmingUp()
    {
        if (!_warmedUp)
        {
            HudManager.GetInstance().UpdateWarmUpText(WarmupCounter);
            if (_warmuUpCounter <= Time.time)
            {
                _warmedUp = true;
                _startNewWave = true;
                HudManager.GetInstance().ShowWarmUpText(false);
                _wavesLeft--;
            }
        }
    }

    /// <summary>
    /// The actual wave coroutine that will spawn the enemies as per scriptable object.
    /// </summary>
    IEnumerator WaveSpawner()
    {
        for (int j = 0; j < _wavesSetup[_waveIndex].SubWaves[_subWaveIndex].Count; j++)
        {
            SpawnEnemy(_wavesSetup[_waveIndex].SubWaves[_subWaveIndex].Enemy);
            _enemyLeftToSpawn--;
            HudManager.GetInstance().UpdateWaveInfoText(_wavesLeft, _enemyLeftToSpawn);
            if (_enemyLeftToSpawn <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(_wavesSetup[_waveIndex].Rate); //how long to spawn an enemy during the wave
        }

        _subWaveIndex++;

        if (_subWaveIndex < _wavesSetup[_waveIndex].SubWaves.Length)
        {
            _startNewWave = true;
        }

        if (_subWaveIndex == _wavesSetup[_waveIndex].SubWaves.Length)
        {
            _subWaveIndex = 0;
            _waveIndex++;
            _wavesLeft--;
            if (_wavesLeft < 0)
            {
                _wavesLeft = 0;
            }
            _waveCounter = _subWaveIndex;

            if (_waveIndex == _wavesSetup.Length)
            {
                LevelManager.GetInstance().LevelCompleted();
                Debug.Log("Level Spawning Completed!");
                this.enabled = false;
            }

            if (_waveIndex < _wavesSetup.Length)
            {
                GetEnemiesLeftToSpawn();
                _currentWave = _waveIndex + 1;
            }
            _counterToNextWave = _timeBetweenWaves;
            _startCounter = true;
        }     
    }

    /// <summary>
    /// Spawn an enemy.
    /// </summary>
    void SpawnEnemy(GameObject enemyToSpawn)
    {
        Instantiate(enemyToSpawn, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
    }

    /// <summary>
    /// Method use to skip the warmup scequence.
    /// </summary>
    public void SkipWarmUp()
    {
        _warmuUpCounter = 0.0f;
    }

    /// <summary>
    /// Get how many enemies there is to spawn in the current wave.
    /// </summary>
    public void GetEnemiesLeftToSpawn()
    {
        _enemyLeftToSpawn = 0;
        _waveCounter = 0;
        for (int i = 0; i < _wavesSetup[_waveIndex].SubWaves.Length; i++)
        {
            _enemyLeftToSpawn += _wavesSetup[_waveIndex].SubWaves[i].Count;
        }
        _waveCounter = _wavesSetup[_waveIndex].SubWaves.Length;
    }

    /// <summary>
    /// Called when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Destroy the Singleton, so it can be recreated from new prefab Spawnpoint.
        _instance = null;
    }
}
