using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
//public class MyEvent : UnityEvent { }

public class OnButtonEvents : MonoBehaviour
{
    [Header("Set the name of you button.")]
    [SerializeField] private string _buttonName;
    [Header("Populate the Events")]
    [SerializeField] private UnityEvent _onButtonCancel;
    [Header("With button sound?")]
    [SerializeField] private bool _playSound = true;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (Input.GetButton(_buttonName))
        {
            _onButtonCancel.Invoke();
            if (_playSound)
            {
                SoundManager.GetInstance().PlaySoundButton();
            }
        }
    }
}
