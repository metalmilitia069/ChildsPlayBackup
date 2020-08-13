using UnityEngine;
using UnityEngine.UI;

public class ClearFieldInput : MonoBehaviour {

    [SerializeField] private InputField _inputField;

    /// <summary>
    /// Erase all text on the input field.
    /// </summary>
    public void ClearField()
    {
        _inputField.text = string.Empty;
    }

}
