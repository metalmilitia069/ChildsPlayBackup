using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Item _itemScript;
    [SerializeField] private ButtonType _typeOfButton;
    [SerializeField] private GameObject _myToolTip;
    private Button _thisButton;

    private int _myIndex;
    public int MyIndex { get => _myIndex; set => _myIndex = value; }
    public ButtonType TypeOfButton { get => _typeOfButton; set => _typeOfButton = value; }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        if (_thisButton == null)
        {
            _thisButton = GetComponent<Button>();
        }
        _myToolTip.SetActive(false);
        Shop.GetInstance().TogglePrice(false);
        if (_itemScript != null)
        {
            _myIndex = _itemScript.IndexInGM; 
        }
    }

    /// <summary>
    /// Called when the object is initialized, but only if the object is active.
    /// Then called every time the object becomes active
    /// </summary>
    private void OnEnable()
    {
        if (_thisButton == null)
        {
            _thisButton = GetComponent<Button>();
        }
        Shop.GetInstance().TogglePrice(false);
        _myToolTip.SetActive(false);
        _thisButton.interactable = true;
        if (_typeOfButton == ButtonType.Buy)
        {
            bool found = false;
            foreach (Item item in LevelManager.GetInstance().CurrentLevelInfo.ItemsAvailable)
            {
                if (item == _itemScript)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                _thisButton.interactable = false;
            }
        }

        if (_typeOfButton == ButtonType.Upgrade)
        {
            bool found = false;
            foreach (Item item in LevelManager.GetInstance().CurrentLevelInfo.ItemsAvailable)
            {
                Item itemSelected = GameManager.GetInstance().SelectedItem;
                if (itemSelected != null)
                {
                    Item itemUP = itemSelected.UpgradeVersion;
                    if (itemUP != null && item == itemUP)
                    {
                        found = true;
                        break;
                    }
                }                
            }
            if (!found)
            {
                _thisButton.interactable = false;
            }
        }
    }

    /// <summary>
    /// When cursor enters above the object
    /// </summary>
    /// <param name="eventData">Unity data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisButton.interactable)
        {
            _myToolTip.SetActive(true);
            Shop.GetInstance().TogglePrice();
            if (_typeOfButton == ButtonType.Buy)
            {
                Shop.GetInstance().TowerSelect(_myIndex); 
            }
            if (!Shop.GetInstance().ChangePrice(_itemScript, _typeOfButton))
            {
                GameManager.GetInstance().DeselectTile();
            }
            SoundManager.GetInstance().PlaySoundOneShot(Sound.OnButtonOver, 0.05f);
        }
    }

    /// <summary>
    /// When cursor exit the object
    /// </summary>
    /// <param name="eventData">Unity data</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _myToolTip.SetActive(false);
        Shop.GetInstance().TogglePrice(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
