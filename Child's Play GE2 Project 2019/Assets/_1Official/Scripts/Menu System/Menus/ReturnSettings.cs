using UnityEngine;
using UnityEngine.UI;

public class ReturnSettings : MonoBehaviour {

    [SerializeField] private int _confirmationPanelIndex;

    private Button _applyButton;

    /// <summary>
    /// Check if the setting has be changed and call the confirmation panel if it was now saved.
    /// </summary>
    /// <param name="panelIndex"></param>
    public void ExitSettings(int panelIndex)
    {
        if (!_applyButton.interactable)
        {
            MenuInteraction.GetInstance().PanelToggle(panelIndex);
        }
        else
        {
            MenuInteraction.GetInstance().PanelToggle(_confirmationPanelIndex);
        }
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {
        _applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
    }

}
