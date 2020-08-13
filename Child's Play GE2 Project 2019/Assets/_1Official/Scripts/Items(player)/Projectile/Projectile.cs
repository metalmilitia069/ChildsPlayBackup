using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _projectileSpeed;
    [SerializeField] protected float _aoeRadius = 0.0f;
    [SerializeField] protected ParticleSystem _impactVFX;

    [SerializeField] protected Transform _target;
    protected Vector3 _direction;
    [SerializeField] protected int _damageValue = 10;
    protected Rigidbody _rigidboby;
    public int DamageValue { get => _damageValue; set => _damageValue = value; }

    [SerializeField] private AudioSource _myAudioSource;

    [SerializeField] private ParticleSystem _projectileFX;

    [SerializeField] private bool _hasHitOnce = false;

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        _rigidboby = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 5.0f);
        if (_impactVFX != null)
        {
            _myAudioSource = _impactVFX.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Assign target
    /// </summary>
    /// <param name="target">GameObject's transform</param>
    public virtual void AssignTarget(Transform target)
    {
        _target = target;
        this.transform.LookAt(_target);
        _rigidboby.velocity = this.transform.forward * _projectileSpeed + _target.GetComponent<NavMeshAgent>().velocity;
        transform.rotation = Quaternion.LookRotation(_rigidboby.velocity);
        Debug.DrawRay(this.transform.position, _rigidboby.velocity, Color.red, 0.5f);
    }
    
    /// <summary>
    /// Called when the object hits a collider.
    /// </summary>
    /// <param name="other">Collider hit</param>
    public virtual void HitTarget(Collider other)
    {
        PlayVFX();

        if (_aoeRadius > 0f)
        {
            Explode();
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                Damage(other.GetComponent<Enemy>());
            }
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// If the projectile has AOE damage
    /// </summary>
    void Explode()
    {
        PlaySound();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _aoeRadius);
        foreach (Collider col in colliders)
        {
            if (col.tag == "Enemy")
            {
                Damage(col.GetComponent<Enemy>());
            }
        }
    }

    /// <summary>
    /// Transfer damage to enemy
    /// </summary>
    /// <param name="enemy"> Enemy to damage. </param>
    public virtual void Damage(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(_damageValue); 
        }
    }

    /// <summary>
    /// Called when the object collides with an other collider/trigger
    /// </summary>
    /// <param name="other">The collider contacted</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_hasHitOnce)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            HitTarget(other);
            _hasHitOnce = true;
        }
        else if (other.gameObject.CompareTag("TilePath") || other.gameObject.CompareTag("Terrain"))
        {
            HitTarget(other);
        }
    }

    /// <summary>
    /// Play the Particle/VFX
    /// </summary>
    public void PlayVFX()
    {
        if (_impactVFX == null)
        {
            return;
        }

        _impactVFX.transform.parent = null;
        _impactVFX.transform.position += Vector3.up;
        _impactVFX.Play();
        Destroy(_impactVFX.gameObject , _impactVFX.main.duration);
        _projectileFX.Play();
    }

    /// <summary>
    /// Play the sound of the audiosource
    /// </summary>
    protected virtual void PlaySound()
    {
        if (_myAudioSource != null)
        {
            if (_myAudioSource.clip != null)
            {
                if (!_myAudioSource.isPlaying)
                {
                    _myAudioSource.PlayOneShot(_myAudioSource.clip);
                }

            }
        }
    }
}

