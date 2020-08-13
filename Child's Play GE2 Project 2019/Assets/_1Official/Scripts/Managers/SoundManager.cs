using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    MoneyIncome,
    OnButtonClick,
    OnButtonOver,
    PlaceTower,
    PlaceBarrier,
    RemoveTower,
    Upgrade,
    SelectTile,
    GameOver,
    WinCompleted,
    LevelCompleted,
    WarmupPhase,
    Music_Level01,
    Music_Level02,
    Music_Level03,
    Music_Level04,
    Music_Level05,
    MissileSfx,
    Spawn,
    Food,
}

[Serializable]
public class SoundAudioclip
{
    [SerializeField] private Sound _sound;
    [SerializeField] private AudioClip _audioClip;

    public Sound Sound { get => _sound; set => _sound = value; }
    public AudioClip AudioClip { get => _audioClip; set => _audioClip = value; }
}


public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager _instance = null;

    public static SoundManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<SoundManager>();
        }
        return _instance;
    }
    #endregion

    [SerializeField] private SoundAudioclip[] _mySounds;
    [SerializeField] private AudioSource _ambientMusic;
    [SerializeField] private AudioSource _uiSfx;

    /// <summary>
    /// Play a sound once
    /// </summary>
    /// <param name="s">what sound to play</param>
    /// <param name="vol"> which volume to play the sound at</param>
    public void PlaySoundOneShot(Sound s , float vol = 1f)
    {
        _uiSfx.PlayOneShot(GetAudioClip(s), vol);
    }

    /// <summary>
    /// play the sound of a button click
    /// </summary>
    public void PlaySoundButton()
    {
        _uiSfx.PlayOneShot(GetAudioClip(Sound.OnButtonClick));
    }

    /// <summary>
    /// Play a sound once, Shop specific
    /// </summary>
    /// <param name="s">what sound to play</param>
    /// <param name="vol"> which volume to play the sound at</param>
    public void PlaySoundOneShotShopOnly(Sound s, float vol = 1f)
    {
        if (!_uiSfx.isPlaying)
        {
            _uiSfx.PlayOneShot(GetAudioClip(s), vol);
        }
    }

    /// <summary>
    /// Choose a music to play
    /// </summary>
    /// <param name="level">Which level is loaded</param>
    public void PlayMusic(int level)
    {
        switch (level)
        {
            case 0:
                _ambientMusic.clip = GetAudioClip(Sound.Music_Level01);
                _ambientMusic.Play();
                break;
            case 1:
                _ambientMusic.clip = GetAudioClip(Sound.Music_Level02);
                _ambientMusic.Play();
                break;
            case 2:
                _ambientMusic.clip = GetAudioClip(Sound.Music_Level03);
                _ambientMusic.Play();
                break;
            case 3:
                _ambientMusic.clip = GetAudioClip(Sound.Music_Level04);
                _ambientMusic.Play();
                break;
            case 4:
                _ambientMusic.clip = GetAudioClip(Sound.Music_Level05);
                _ambientMusic.Play();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// stop the music that is playing
    /// </summary>
    public void StopMusic()
    {
        _ambientMusic.Stop();
    }

    /// <summary>
    /// Return the audio clip that is store
    /// </summary>
    /// <param name="s">what clip to get</param>
    /// <returns>the audioclip found</returns>
    public AudioClip GetAudioClip(Sound s)
    {
        foreach (SoundAudioclip audioClip in _mySounds)
        {
            if (audioClip.Sound == s)
            {
                return audioClip.AudioClip;
            }
        }
        Debug.LogError("Sound " + s + " not found");
        return null;
    }
    
}


