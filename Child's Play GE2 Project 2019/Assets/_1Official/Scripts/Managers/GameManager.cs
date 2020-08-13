using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is to be place in the GameManager script
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance = null;

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<GameManager>();
        }
        return _instance;
    }
    #endregion

    [Header("Selection Cursor Objects")]
    [SerializeField] private GameObject _tileSelectionCursor;
    [SerializeField] private GameObject _tileSelectedCursor;
    [Header("Item Prefabs")]
    [SerializeField] private GameObject[] _listOfTower;
    [SerializeField] private GameObject[] _listOfTowerPlaceHolder;
    [SerializeField] private GameObject[] _listOfBarrier;
    [SerializeField] private GameObject[] _listOfBarrierPlaceHolder;
    
    private int _selectedTowerIndex = 0;
    private int _selectedBarrierIndex = 0;

    [Header("Panels Indexes")]
    [SerializeField] private int _newRankPanelIndex = 4;
    [SerializeField] private int _gameOverPanelIndex = 8;
    [SerializeField] private int _scorePanelIndex = 9;
    [SerializeField] private int _winPanelIndex = 10;
    public int NewRankPanelIndex { get => _newRankPanelIndex; }
    public int ScorePanelIndex { get => _scorePanelIndex; }
    public int WinPanelIndex { get => _winPanelIndex; }

    [Header("Game Options")]
    [Header("What are the two Timescale multiplier ")]
    [SerializeField] private float _speedMulOne;
    [SerializeField] private float _speedMulTwo;
    public float SpeedMulOne { get => _speedMulOne; }
    public float SpeedMulTwo { get => _speedMulTwo; }
    private FastForwardButton _fastForwardButton;
    public FastForwardButton FastForwardButton
    {
        get
        {
            if (_fastForwardButton == null)
            {
                _fastForwardButton = GameObject.FindObjectOfType<FastForwardButton>();
            }
            return _fastForwardButton;
        }
    }

    private float _currentGameSpeed = 1.0f; 
    public float CurrentGameSpeed { get => _currentGameSpeed; set => _currentGameSpeed = value; }

    private bool _showHealthBars = true;
    public bool ShowHealthBars { get => _showHealthBars; }

    private ItemTile _selectedTile;
    public ItemTile SelectedTile { get => _selectedTile; }
    public GameObject TileSelectionCursor { get => _tileSelectionCursor; }


    /// <summary>
    /// Return the item from the selected tile or null if there is no items.
    /// </summary>
    public Item SelectedItem
    {
        get
        {
            if (_selectedTile != null)
            {
                if (_selectedTile.CurrentItem != null)
                {
                    return _selectedTile.CurrentItem.GetComponent<Item>();
                } 
            }
            return null;
        }
    }

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    void Start()
    {
        PlaceHoldersAndCursorsInit();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && _selectedTile != null)
        {
            DeselectTile();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            if (MenuInteraction.GetInstance().AtDefaultOrRootPanel)
            {
                PauseManager.GetInstance().ToggleMainMenu();
            }
        }
    }
    
    /// <summary>
    /// Deselect the current selected tile,
    /// hide all placeholder items
    /// and call the method to update the selected tile text.
    /// </summary>
    public void DeselectTile()
    {
        _tileSelectedCursor.SetActive(false);

        HidePlaceHolders();
        ShowRange(false);
        _selectedTile = null;
        Shop.GetInstance().SetPanelActive(Shop.GetInstance().PLACEHOLDER);
    }

    /// <summary>
    /// Hide or show the range of the currently select item as it selects or deselect the tiles
    /// </summary>
    /// <param name="show">showing state, default = true</param>
    private void ShowRange(bool show = true)
    {
        if (_selectedTile != null)
        {
            if (_selectedTile.CurrentItem != null)
            {
                GameObject rangeGO = _selectedTile.CurrentItem.GetComponent<Item>().RangeGO;
                if (rangeGO != null)
                {
                    rangeGO.SetActive(show);
                }
                if (!show)
                {
                    GameObject rangeGOUpgrade = _selectedTile.CurrentItem.GetComponent<Item>().RangeGOUpgrade;
                    if (rangeGOUpgrade != null)
                    {
                        rangeGOUpgrade.SetActive(show);
                    }
                }
                
            }
        }
    }

    /// <summary>
    /// Initialize the prefabs needed for the game.
    /// </summary>
    void PlaceHoldersAndCursorsInit()
    {
        for (int i = 0; i < _listOfBarrierPlaceHolder.Length; i++)
        {
            _listOfBarrierPlaceHolder[i] = Instantiate(_listOfBarrierPlaceHolder[i]);
            _listOfBarrierPlaceHolder[i].SetActive(false);
        }

        for (int i = 0; i < _listOfTowerPlaceHolder.Length; i++)
        {
            _listOfTowerPlaceHolder[i] = Instantiate(_listOfTowerPlaceHolder[i]);
            _listOfTowerPlaceHolder[i].SetActive(false);
        }

        HidePlaceHolders();

        _tileSelectionCursor.SetActive(false);
        _tileSelectedCursor.SetActive(false);
    }

    /// <summary>
    /// Select a tile and sets it as the tile selected.
    /// </summary>
    /// <param name="tile">Tile to select</param>
    public void TileSelection(ItemTile tile)
    {
        HidePlaceHolders();
        ShowCursorOnTile(_tileSelectedCursor, tile);
        ShowRange(false);
        _selectedTile = tile;
        if (tile.CurrentItem != null)
        {
            Shop.GetInstance().SetPanelActive(Shop.GetInstance().UPGRADESELLPANEL);
            ShowRange(true);
            return;
        }

        switch (tile.TileType)
        {
            case TileType.Tower:
                Shop.GetInstance().SetPanelActive(Shop.GetInstance().SHOPPANEL);
                ShowItemOnTile(_listOfTowerPlaceHolder[_selectedTowerIndex], tile);
                break;
            case TileType.Barrier:
                Shop.GetInstance().SetPanelActive(Shop.GetInstance().BARRIERPANEL);
                ShowItemOnTile(_listOfBarrierPlaceHolder[_selectedBarrierIndex], tile);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Hides all the item's place holders
    /// </summary>
    private void HidePlaceHolders()
    {
        foreach (var item in _listOfBarrierPlaceHolder)
        {
            item.SetActive(false);
        }
        foreach (var item in _listOfTowerPlaceHolder)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// Show cursor on tile.
    /// </summary>
    /// <param name="cursor">Cursor to show</param>
    /// <param name="tile">Tile to show cursor on</param>
    public void ShowCursorOnTile(GameObject cursor, ItemTile tile)
    {
        _tileSelectedCursor.SetActive(false);
        _tileSelectionCursor.SetActive(false);
        if (tile == null)
        {
            return;
        }
        cursor.SetActive(true);
        cursor.transform.position = tile.transform.position;
        cursor.transform.rotation = tile.transform.rotation;
        cursor.transform.position += Vector3.up * 3.01f;
    }

    /// <summary>
    /// Show the selected item on the selected tile as preview.
    /// </summary>
    /// <param name="item">Item to show</param>
    /// <param name="tile">Selected Tile</param>
    private void ShowItemOnTile(GameObject item, ItemTile tile)
    {
        if (tile == null)
        {
            return;
        }
        item.SetActive(true);
        item.transform.position = tile.transform.position;
        item.transform.rotation = tile.transform.rotation;
        item.transform.position += Vector3.up * 3.0f;
    }

    /// <summary>
    /// Place an item on the selected tile if it is empty.
    /// </summary>
    public void PlaceItem()
    {
        if (_selectedTile == null)
        {
            Debug.Log("No Tile Selected.");
            return;
        }
        if (_selectedTile.CurrentItem != null)
        {
            UpgradeItem();
            return;
        }

        switch (_selectedTile.TileType)
        {
            case TileType.Tower:
                if (!MoneyManager.GetInstance().TryToBuy(_listOfTower[_selectedTowerIndex].GetComponent<Item>().Value))
                {
                    return;
                }
                SoundManager.GetInstance().PlaySoundOneShot(Sound.PlaceTower, 0.5f);
                InstantiateItemOnTile(_listOfTower[_selectedTowerIndex]);
                break;
            case TileType.Barrier:
                if (!MoneyManager.GetInstance().TryToBuy(_listOfBarrier[_selectedBarrierIndex].GetComponent<Item>().Value))
                {
                    return;
                }
                SoundManager.GetInstance().PlaySoundOneShot(Sound.PlaceBarrier, 0.5f);
                InstantiateItemOnTile(_listOfBarrier[_selectedBarrierIndex]);
                break;
            default:
                break;
        }
        DeselectTile();
    }

    /// <summary>
    /// Tries to upgrade the select Item.
    /// </summary>
    private void UpgradeItem()
    {
        Item upgradeVersion = _selectedTile.CurrentItem.GetComponent<Item>().UpgradeVersion;
        if (upgradeVersion != null)
        {
            if (!MoneyManager.GetInstance().TryToBuy(upgradeVersion.Value))
            {
                Debug.Log("Not enough money for Upgrade!");
                return;
            }
            else
            {
                Debug.Log("Upgrading!");
                Destroy(_selectedTile.CurrentItem);
                SoundManager.GetInstance().PlaySoundOneShot(Sound.Upgrade, 0.5f);
                InstantiateItemOnTile(upgradeVersion.gameObject);
            }
        }
        else
        {
            Debug.Log("No upgrade version, available.");
        }
        DeselectTile();

    }

    /// <summary>
    /// Remove and destroy the Item on the current selected tile, and call the sell method.
    /// </summary>
    public void RemoveItem()
    {
        if (_selectedTile == null)
        {
            Debug.Log("No Tile Selected.");
            return;
        }
        if (_selectedTile.CurrentItem == null)
        {
            Debug.Log("No Item on the current selected Tile.");
            DeselectTile();
            return;
        }
        MoneyManager.GetInstance().MoneyChange(_selectedTile.CurrentItem.GetComponent<Item>().Value); //Sell item
        SoundManager.GetInstance().PlaySoundOneShot(Sound.RemoveTower, 1f);
        Destroy(_selectedTile.CurrentItem.gameObject);
        _selectedTile.CurrentItem = null;
        Shop.GetInstance().SetPanelActive(Shop.GetInstance().PLACEHOLDER);

        DeselectTile();
    }
    
    /// <summary>
    /// Instantiate the Item passed as a param on the currently selected tile.
    /// </summary>
    /// <param name="item">Item to instantiace</param>
    public void InstantiateItemOnTile(GameObject item)
    {
        _selectedTile.CurrentItem =
                    Instantiate(
                        item,
                        _selectedTile.transform.position + Vector3.up * 3.0f,
                        _selectedTile.transform.rotation,
                        LevelManager.GetInstance().CurrentLevelGO.transform // Childd of the Level
                        );
        HidePlaceHolders();
    }

    /// <summary>
    /// When a store Button is Pressed this method works its magic
    /// </summary>
    public void SwapPlaceHoldersOnTile()
    {
        HidePlaceHolders();
        ShowItemOnTile(_listOfTowerPlaceHolder[_selectedTowerIndex], _selectedTile);
        TileSelection(_selectedTile);
    }

    /// <summary>
    /// Change the selected tower index
    /// </summary>
    /// <param name="index">Index of the tower</param>
    public void SetTowerSelectionIndex(int index)
    {
        _selectedTowerIndex = index;
    }

    /// <summary>
    /// Toggles the Cavas panels as per incoming index
    /// </summary>
    /// <param name="index">Panel index to display</param>
    public void PanelSelection(int index)
    {
        MenuInteraction.GetInstance().PanelToggle(index);
    }

    /// <summary>
    /// Display the Game Over screen
    /// </summary>
    public void GameOver()
    {
        Input.ResetInputAxes();
        Cursor.lockState = CursorLockMode.None;
        SoundManager.GetInstance().PlaySoundOneShot(Sound.GameOver);
        PauseManager.GetInstance().PauseGame();
        PanelSelection(_gameOverPanelIndex);
    }

    /// <summary>
    /// Show or Hide the enemies health bars.
    /// </summary>
    public void ToggleHealthBars()
    {
        _showHealthBars = !_showHealthBars;
        foreach (var item in EnemyManager.GetInstance().ListOfEnemies)
        {
            if (!(item is EnemyWorker || item is EnemyFlyer))
            {
                item.HealthBar.gameObject.SetActive(_showHealthBars);
            }
        }
    }
    

}
