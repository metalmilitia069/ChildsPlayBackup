using UnityEngine;
using UnityEngine.UI;

public class NewRank : MonoBehaviour
{
    [SerializeField] private Text _newRankText;
    [SerializeField] private InputField _inputField;

    /// <summary>
    /// Called when the object is initialized, but only if the object is active.
    /// Then called every time the object becomes active
    /// </summary>
    void OnEnable()
    {
        _newRankText.text = "You got the highscore!\nEnter your initials";
    }

    /// <summary>
    /// Set the new rank base on what is on the input field.
    /// </summary>
    public void SetNewRank()
    {
        if (_inputField.text == string.Empty)
        {
            Settings.GetInstance().SetLoaderboard();
        }
        else
        {
            Settings.GetInstance().SetLoaderboard(_inputField.text.ToUpper());
        }
        _inputField.text = string.Empty;
    }
}
