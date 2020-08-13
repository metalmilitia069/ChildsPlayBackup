using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Transform _towerTarget;

    [SerializeField] private Transform _pivot;
    [SerializeField] private float _rotationSpeed = 10f;

    //FIRING PART
    [Header("Tower Option")]
    [SerializeField] private bool _lookAtTarget = false;
    private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;

    private float _range;// = 15f;
    private float _rateOfFire;// = 1f;
    private float _countdownToNextFire = 0f;
    private float _innerRadius;

    [SerializeField] private Tower_SO _tower_SO;

    [Header("Tower VFX")]
    [SerializeField] private Light _lightEffect;
    [SerializeField] private ParticleSystem _vfxLaser;
    [SerializeField] private LineRenderer _lineRendererComponent;
    [SerializeField] private ParticleSystem _firingVFX;

    [SerializeField] private int levelUpgradeIndex = 0;
    private AudioSource _myAudioSource;
 
    public Transform ProjectileSpawnPoint { get { return _projectileSpawnPoint; } }
    public Transform TowerTarget { get { return _towerTarget; } }
    public float Range { get => _tower_SO.TowerLevelsArray[levelUpgradeIndex].range; }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Start()
    {
        _myAudioSource = GetComponent<AudioSource>();
        if (_tower_SO.towerType == ProjectTileType.Laser)
        {
            _lightEffect = GetComponentInChildren<Light>();
            _vfxLaser = GetComponentInChildren<ParticleSystem>();
            _lineRendererComponent = GetComponentInChildren<LineRenderer>();
        }

        SO_Reference();
    }

    /// <summary>
    /// Get Scriptable object data
    /// </summary>
    void SO_Reference()
    {        
         _range = _tower_SO.TowerLevelsArray[levelUpgradeIndex].range;
         _rateOfFire = _tower_SO.TowerLevelsArray[levelUpgradeIndex].bulletPerSecond;
         _projectilePrefab = _tower_SO.TowerLevelsArray[levelUpgradeIndex].projectilePrefab;
    }

    /// <summary>
    /// Update the target of the tower.
    /// </summary>
    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;

        Enemy nearestEnemy = null;

        foreach (Enemy enemy in EnemyManager.GetInstance().ListOfEnemies)
        {
            if (enemy.GetComponent<Enemy>().IsDying)
            {
                continue;
            }
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                        
            if (distanceToEnemy < _range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
                break;
            }
        }

        if(nearestEnemy != null && shortestDistance <= _range && shortestDistance >= _innerRadius)
        {
            _towerTarget = nearestEnemy.transform;
        }
        else
        {
            _towerTarget = null;
        }
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        UpdateTarget();
        ShootAndLookAtTarget();
    }

    /// <summary>
    /// Pivot the tower and shoot at its target
    /// </summary>
    private void ShootAndLookAtTarget()
    {
        _countdownToNextFire -= Time.deltaTime;
        if (_towerTarget == null)
        {
            StopLaser();
            return;
        }

        //Target Lock
        if (_lookAtTarget)
        {
            _pivot.LookAt(_towerTarget);
        }
        else
        {
            Vector3 direction = _towerTarget.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(_pivot.rotation, lookRotation, _rotationSpeed * Time.deltaTime).eulerAngles;
            _pivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        //FIRING PART


        if (_countdownToNextFire <= 0f)
        {
            Shoot();
            _countdownToNextFire = 1f / _rateOfFire;
        }
        if (_tower_SO.towerType == ProjectTileType.Laser)
        {
            FireLaserBeam();
        }
    }

    /// <summary>
    /// Shoot at the target, spawn the projectile
    /// </summary>
    private void Shoot()
    {

        GameObject projectileGameObject = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.AssignTarget(_towerTarget);
        PlaySound();
        PlayVFX();
    }

    /// <summary>
    /// Play the sound on the audiosource
    /// </summary>
    private void PlaySound()
    {
        if (_myAudioSource.clip != null)
        {
            if (!_myAudioSource.isPlaying)
            {
                _myAudioSource.PlayOneShot(_myAudioSource.clip);
            }
        }
    }

    /// <summary>
    /// Play the particle VFX
    /// </summary>
    private void PlayVFX()
    {
        //Do vfx 
        if (_firingVFX == null)
        {
            return;
        }
        _firingVFX.Play();
    }

    /// <summary>
    /// Draw a gizmo of the range on the scene editor
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _tower_SO.TowerLevelsArray[levelUpgradeIndex].range);
        Gizmos.DrawWireSphere(this.transform.position, _innerRadius);        
    }

    /// <summary>
    /// used for the laser tower, it activate its laser beem VFX
    /// </summary>
    public void FireLaserBeam()
    {
        //GRAPHICS PART
        _lineRendererComponent.enabled = true;

        _lineRendererComponent.SetPosition(0, this.ProjectileSpawnPoint.position);
        _lineRendererComponent.SetPosition(1, this.TowerTarget.position);
        
        if (!this._vfxLaser.isPlaying)
        {
            this._vfxLaser.Play();
        }    
        this._lightEffect.enabled = true;
        Vector3 direction = this.ProjectileSpawnPoint.position - this.TowerTarget.position;
        this._vfxLaser.transform.position = this.TowerTarget.position + direction.normalized;
        this._vfxLaser.transform.rotation = Quaternion.LookRotation(direction);
        
    }

    /// <summary>
    /// Stop the rendering of the laser VFX
    /// </summary>
    public void StopLaser()
    {

        if (_tower_SO.towerType == ProjectTileType.Laser)
        {
            _lineRendererComponent.enabled = false;
            this._vfxLaser.Stop();
            this._lightEffect.enabled = false;
            _myAudioSource.Stop();
        }
    }
}
