using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour {

    private AudioSource _audioSource;

    public AudioSource AudioSource { get { return _audioSource; } set { _audioSource = value; } }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {
        _audioSource = GetComponent<AudioSource>();
	}
	
}
