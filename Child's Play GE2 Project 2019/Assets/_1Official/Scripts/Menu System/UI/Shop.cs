using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonType
{
    Buy,
    Sell,
    Upgrade
}

public class Shop : MonoBehaviour
{
    #region Singleton
    private static Shop _instance = null;

    public static Shop GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<Shop>();
        }
        return _instance;
    }
    #endregion
    
    [SerializeField] private GameObject[] _panels;
    [SerializeField] private Text _priceT;
    [SerializeField] private Text _priceB;
    [SerializeField] private Text _priceU;
    [SerializeField] private GameObject _boundary;
   
    private const int _PLACEHOLDER = 0;
    private const int _SHOPPANEL = 1;
    private const int _UPGRADESELLPANEL = 2;
    private const int _BARRIERPANEL = 3;
    private int _currentPanel = 1;
    private bool _onButton;
    private Vector3 _rootPos;

    public GameObject[] Panels { get => _panels; set => _panels = value; }
    public int PLACEHOLDER { get => _PLACEHOLDER; }
    public int SHOPPANEL { get => _SHOPPANEL;}
    public int UPGRADESELLPANEL { get => _UPGRADESELLPANEL; }
    public int BARRIERPANEL { get => _BARRIERPANEL; }
    public bool OnButton { get => _onButton; set => _onButton = value; }

    /// <summary>
    /// Show the active shop panel and hide the others
    /// </summary>
    /// <param name="panelIndex">Panel to set active</param>
    public void SetPanelActive(int panelIndex)
    {
        if (panelIndex != _PLACEHOLDER)
        {
            SoundManager.GetInstance().PlaySoundOneShotShopOnly(Sound.SelectTile, 0.5f);
        }
        _currentPanel = panelIndex;
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(panelIndex == i);
        }
    }

    /// <summary>
    /// Move the panel to the position where the player clicked
    /// </summary>
    public void MoveToClick()
    {
        _rootPos = Input.mousePosition;
        Panels[_currentPanel].transform.position = _rootPos;
    }

    /// <summary>
    /// Set what item the player is looking to buy
    /// </summary>
    /// <param name="index">the item index</param>
    public void TowerSelect(int index)
    {
        GameManager.GetInstance().SetTowerSelectionIndex(index);
        GameManager.GetInstance().SwapPlaceHoldersOnTile();
    }

    /// <summary>
    /// Change the price on the tooltip
    /// </summary>
    /// <param name="item">the item to retreive the price from</param>
    /// <param name="buttonType">the type of the button that is asking to change the price</param>
    /// <returns></returns>
    public bool ChangePrice(Item item, ButtonType buttonType)
    {
        if (buttonType == ButtonType.Buy)
        {
            if (_priceT.IsActive())
            {
                _priceT.text = $"Price\n{item.Value.ToString()}";
            }
            else _priceB.text = $"Price\n{item.Value.ToString()}";
        }
        else if (buttonType == ButtonType.Upgrade)
        {
            Item itemOnTile = GameManager.GetInstance().SelectedItem;
            _priceU.text = $"Cost\n{itemOnTile.UpgradeVersion.Value.ToString()}";
        }
        else
        {
            Item itemOnTile = GameManager.GetInstance().SelectedItem;
            if (itemOnTile == null)
            {
                return false;
            }
            _priceU.text = $"Value\n{itemOnTile.Value.ToString()}";
        }
        return true;
    }

    /// <summary>
    /// Show or hide the prices tooltip
    /// </summary>
    /// <param name="show">show or hide</param>
    public void TogglePrice(bool show = true)
    {
        _priceT.enabled = show;
        _priceB.enabled = show;
        _priceU.enabled = show;
    }
}
