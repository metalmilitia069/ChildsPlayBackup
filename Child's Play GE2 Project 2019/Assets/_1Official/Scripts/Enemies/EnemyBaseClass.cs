using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(
    typeof(EnemyAnimation),
    typeof(EnemyMovementMechanics)
    )]
public class EnemyBaseClass : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _hitPoints = 100;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackSpeed = 0.5f;
    [SerializeField] private int _foodBites = 5;
    [SerializeField] protected int _value = 10;

    [Header("Particle effects")]
    [SerializeField] private ParticleSystem _spawnVFX;
    [SerializeField] private ParticleSystem _dieVFX;
    [SerializeField] private ParticleSystem _eatVFX;

    protected bool hasFocus = false; //Used for camera focus
    private EnemyAnimation _enemyAnimation;
    private Item _target;
    private GameObject _targetGO;
    protected bool _isAttacking = false;

    protected bool _isDying = false;
    private bool _asEaten = false;

    protected EnemyMovementMechanics eMMCode;

    public int HitPoints { get => _hitPoints; set => _hitPoints = value; }
    public bool IsDying { get => _isDying; }
    public bool HasFocus { get => hasFocus; set => hasFocus = value; }

    private int _currentDamageOvertime;

    [Header("Health Bar")]
    [SerializeField] private Color startingHealthColor;
    [SerializeField] private Color endHealthColor;
    private Image _healthBar;
    protected float _ogHP;
    public Image HealthBar { get => _healthBar; set => _healthBar = value; }
    public EnemyMovementMechanics EMMCode { get => eMMCode; }

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        _enemyAnimation = GetComponent<EnemyAnimation>();
        eMMCode = GetComponent<EnemyMovementMechanics>();
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        SetAnimWalking();

        EnemyManager.GetInstance().AddEnemyToList(this as Enemy);
        _healthBar = GetComponentInChildren<Image>();
        _healthBar.gameObject.SetActive(GameManager.GetInstance().ShowHealthBars);
        _ogHP = _hitPoints;
    }

    /// <summary>
    /// Called when enemy takes damage
    /// </summary>
    /// <param name="damageValue">Damage received</param>
    public void TakeDamage(int damageValue)
    {
        this._hitPoints -= damageValue;
        UpdateHealthBar();
        if (_hitPoints <= 0)
        {
            _dieVFX.Play();
            Die();
            
        }
    }

    /// <summary>
    /// Call animation script
    /// </summary>
    public void SetAnimWalking()
    {
        _enemyAnimation.SetWalking();
    }

    /// <summary>
    /// Call animation script
    /// </summary>
    public void SetAnimAttacking()
    {
        _enemyAnimation.SetAttacking();
    }

    /// <summary>
    /// Call animation script
    /// </summary>
    public void SetAnimDizzy()
    {
        _enemyAnimation.SetDizzy();
    }

    /// <summary>
    /// Call animation script
    /// </summary>
    public void SetAnimRetreating()
    {
        _enemyAnimation.SetRetreating();
    }

    /// <summary>
    /// Called when enemy dies.
    /// </summary>
    protected virtual void Die()
    {
        _isDying = true;

        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }

        eMMCode.NavMeshAgent.enabled = false;
        SetAnimRetreating();

        EnemyManager.GetInstance().RemoveEnemyFromList(this as Enemy);
        MoneyManager.GetInstance().MoneyChange(_value);
        SoundManager.GetInstance().PlaySoundOneShot(Sound.MoneyIncome, 0.05f);
        ScoreManager.GetInstance().EnemyKilled++;
        ScoreManager.GetInstance().MoneyEarned += _value;
    }

    /// <summary>
    /// Call when enemy start attacking
    /// </summary>
    /// <param name="target"></param>
    public virtual void SetAttacking(Item target)
    {
        _isAttacking = true;
        this._targetGO = target.gameObject;
        this._target = target;
        SetAnimAttacking();
        eMMCode.AttackStance(target.transform.position);
        StartCoroutine(AttackCo());
    }

    /// <summary>
    /// Coroutine of the enemy attacking
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCo()
    {
        while (_isAttacking)
        {
            if (_targetGO == null)
            {
                _target = null;
                _isAttacking = false;
                ResumeWalking();
                break;
            }
            _target.TakeDamage(_damage);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    /// <summary>
    /// Called after the enemy wants to resume walking
    /// </summary>
    private void ResumeWalking()
    {
        SetAnimWalking();
        eMMCode.MoveOnStance();
    }

    /// <summary>
    /// Called when the enemy enters a trigger.
    /// </summary>
    /// <param name="other">The collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_targetGO == null)
        {
            if (other.CompareTag("Item"))
            {
                SetAttacking(other.GetComponent<Item>());
            }
        }
        if (other.CompareTag("Food"))
        {
            if (!_asEaten)
            {
                EatFood(other.GetComponent<Food>());
            } 
        }
    }

    /// <summary>
    /// Called when the enemy eat food.
    /// </summary>
    /// <param name="_food">food that enemy is eating</param>
    private void EatFood(Food _food)
    {
        _eatVFX.Play();
        _asEaten = true;
        _food.TakeDamage(_foodBites);
    }

    /// <summary>
    /// Called when the enemy escaped the level after eating.
    /// </summary>
    public void LeaveWithFood()
    {
        ScoreManager.GetInstance().EnemyEscaped++;
        EnemyManager.GetInstance().RemoveEnemyFromList(this as Enemy);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Used to start the damage over time coroutine.
    /// </summary>
    /// <param name="damageValue">Damage per tick</param>
    /// <param name="tickSpeed">Time between ticks</param>
    /// <param name="lastTime">The time the coroutine last</param>
    public void DamageOverTime(int damageValue, float tickSpeed, float lastTime)
    {
        StartCoroutine(DamageOverTimeRoutine(damageValue, tickSpeed, lastTime));
    }

    /// <summary>
    /// The coroutine that damages the enemy over time.
    /// </summary>
    /// <param name="damageValue">Damage per tick</param>
    /// <param name="tickSpeed">Time between ticks</param>
    /// <param name="lastTime">The time the coroutine last</param>
    /// <returns></returns>
    private IEnumerator DamageOverTimeRoutine(int damageValue, float tickSpeed, float lastTime)
    {
        float currentTime = Time.time;
        float endTime = currentTime + lastTime;
        while (currentTime <= endTime && this._hitPoints > 0)
        {
            this.TakeDamage(damageValue);
            yield return new WaitForSeconds(tickSpeed);
            currentTime = Time.time;
        }
    }

    /// <summary>
    /// Updates the healthbar image.
    /// </summary>
    protected virtual void UpdateHealthBar()
    {
        if (_healthBar == null)
        {
            return;
        }
        _healthBar.fillAmount = _hitPoints / _ogHP;
        _healthBar.color = Color.Lerp(endHealthColor, startingHealthColor, _healthBar.fillAmount);
    }
}
