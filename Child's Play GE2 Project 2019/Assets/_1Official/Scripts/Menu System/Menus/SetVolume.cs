using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum VolumeTypes { Master, Music, SFX }

[RequireComponent(typeof(Slider))]
public class SetVolume : MonoBehaviour {
    [SerializeField] private VolumeTypes _volumeType;
    [SerializeField] private AudioMixer _audioMixer;
    private string _nameParam;

    private float _prefValue;
    private Slider _slider;
    public Slider Slider { get { return _slider; } }
    private Button _applyButton;

    /// <summary>
    /// Set the volume based on the slider value
    /// </summary>
    /// <param name="sliderValue">Slider value</param>
    public void SetVol(float sliderValue)
    {
        _audioMixer.SetFloat(_nameParam, sliderValue);
        if (_prefValue != _slider.value)
        {
            _applyButton.interactable = true;
        }
        _slider.value = sliderValue;
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {

        switch (_volumeType)
        {
            case VolumeTypes.Master:
                _nameParam = Settings.GetInstance().MasterVolParam;
                _prefValue = Settings.GetInstance().MasterVolValue;
                break;
            case VolumeTypes.Music:
                _nameParam = Settings.GetInstance().MusicVolParam;
                _prefValue = Settings.GetInstance().MusicVolValue;
                break;
            case VolumeTypes.SFX:
                _nameParam = Settings.GetInstance().SFXVolParam;
                _prefValue = Settings.GetInstance().SFXVolValue;
                break;
            default:
                break;
        }

        _applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        _slider = GetComponent<Slider>();
        _slider.value = _prefValue;
    }

}
