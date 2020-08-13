///
/// Author: Alexandre Lepage
/// Date: October 2018
/// Desc: Project for LaSalle College
///

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour {

    #region Singleton
    private static MenuInteraction _instance = null;

    public static MenuInteraction GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<MenuInteraction>();
        }
        return _instance;
    }
    #endregion

    [Header("Menus")]
    [SerializeField] private int _sceneDefaultIndex = 0;
    [SerializeField] private GameObject[] _panels;
    public GameObject[] Panels { get { return _panels; } }
    [SerializeField] private Selectable[] _defaultSelections;
    public Selectable[] DefaultSelections { get { return _defaultSelections; } }

    private int _currentPanel;
    public bool AtDefaultOrRootPanel { get => _currentPanel == _sceneDefaultIndex || _currentPanel == 0; }

    private List<Selectable> _selectables;


    // Level Selection
    [Header("Level Selection Specific")]
    [SerializeField] private Button[] levelButton;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {
        _selectables = Selectable.allSelectables;
        PanelToggle(_sceneDefaultIndex);
    }

    /// <summary>
    /// Update is called once per frame 
    /// </summary>
    void Update () {
        _selectables = Selectable.allSelectables;
        bool oneSelected = false;
        foreach (var item in _selectables)
        {
            if (item.GetComponent<SelectableInteraction>().Selected)
            {
                oneSelected = true;
                break;
            }
        }
        if (!oneSelected || _selectables.Count == 0)
        {
            _defaultSelections[_currentPanel].Select();
        }
	}

    /// <summary>
    /// Toggle to the scene's default panel.
    /// </summary>
    public void PanelToggle()
    {
        PanelToggle(_sceneDefaultIndex);
    }

    /// <summary>
    /// Show or hide canvas panels as per index param.
    /// </summary>
    /// <param name="panelIndex">Panel index</param>
    public void PanelToggle(int panelIndex)
    {
        _currentPanel = panelIndex;
        Input.ResetInputAxes();
        
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(panelIndex == i);
            if (panelIndex == i)
            {
                _defaultSelections[i].Select();
            }
        }
        UnlockedLevelsCheck();
    }

    /// <summary>
    /// Call the application to quit, (exit the game) 
    /// Or stop playing in editor.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Check which level the player has unclocked as per his or her progression.
    /// </summary>
    public void UnlockedLevelsCheck()
    {
        int count = 0;
        foreach (Button button in levelButton)
        {
            button.interactable = count++ <= Settings.GetInstance().LevelsUnlocked;
        }
    }
}
