using UnityEngine;
using UnityEngine.UI;

public class ButtonToInputField : MonoBehaviour {

    [SerializeField] private InputField _inputField;
    private string _letter;
   
    /// <summary>
    /// Send the detected letter to the Input field.
    /// </summary>
    public void SendLetter()
    {
        _inputField.text += _letter;
    }

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    void Start () {
        _letter = this.name[0].ToString();
	}

}
