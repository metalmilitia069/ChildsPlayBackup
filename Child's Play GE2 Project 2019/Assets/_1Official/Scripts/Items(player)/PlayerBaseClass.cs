using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseClass : MonoBehaviour
{
    [Header("Player Option")]
    [SerializeField] private int _hitPoints = 100;
    public int HitPoints { get => _hitPoints; set => _hitPoints = value; }

    protected AudioSource myAudioSource;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    protected void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when the hitPoints drop to zero
    /// </summary>
    protected virtual void Die()
    {
        if (this == GameManager.GetInstance().SelectedItem)
        {
            GameManager.GetInstance().DeselectTile();
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Called to reduce hitPoints
    /// </summary>
    /// <param name="damageValue">Incoming Damage</param>
    public virtual void TakeDamage(int damageValue)
    {
        this._hitPoints -= damageValue;

        if (this.HitPoints <= 0)
        {
            Die();
            return;
        }
        PlaySound();
    }

    /// <summary>
    /// Play sound on the object AudioSource
    /// </summary>
    protected void PlaySound()
    {
        if (myAudioSource.clip != null && !myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(myAudioSource.clip);
        }
    }

}
