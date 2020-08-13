///
/// Author: Alexandre Lepage
/// Date: October 2018
/// Desc: Project for LaSalle College
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    [Header("Graphics")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Text _percentText;
    [Header("Options")]
    [SerializeField] private bool _waitForUserInput = false;
    [SerializeField] private float _waitForTimeDelay = 0.0f;
    [Header("Scene to load (-1 = next in build index)")]
    [SerializeField] private int _sceneIndex = -1;

    private AsyncOperation _asyncLoader;


    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {
        Time.timeScale = 1.0f;
        Input.ResetInputAxes();
        System.GC.Collect();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (_sceneIndex != -1 && _sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            _asyncLoader = SceneManager.LoadSceneAsync(_sceneIndex);
        }
        else
        {
            if (currentScene == SceneManager.sceneCountInBuildSettings - 1)
            {
                _asyncLoader = SceneManager.LoadSceneAsync(0); 
            }
            else
            {
                _asyncLoader = SceneManager.LoadSceneAsync(currentScene + 1);
            }
        }
        if (_waitForUserInput)
        {
            _asyncLoader.allowSceneActivation = false;
        }
        else if (_waitForTimeDelay > 0)
        {
            _asyncLoader.allowSceneActivation = false;
            Invoke("TimeDelay", _waitForTimeDelay);
        }
    }
	
    /// <summary>
	/// Update is called once per frame 
    /// </summary>
	void Update () {

        if (Input.anyKey && _waitForUserInput)
        {
            _asyncLoader.allowSceneActivation = true;
        }
        
        _fillImage.fillAmount = _asyncLoader.progress + 0.1f;

        if (_fillImage.fillAmount <= 0.99f)
        {
            _percentText.text = "Loading " + _fillImage.fillAmount.ToString("P2");
        }
        else if (_waitForUserInput)
        {
            _percentText.text = "Press any key to continue.";
        }
        else
        {
            _percentText.text = _fillImage.fillAmount.ToString("P2");
        }
    }

    /// <summary>
    /// To set the scene loading on a time delay loading is completed.
    /// </summary>
    private void TimeDelay()
    {
        _asyncLoader.allowSceneActivation = true;
    }
}
