using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _rangeGOUpgrade;
    private Button _thisButton;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Start()
    {
        _thisButton = GetComponent<Button>();
    }

    /// <summary>
    /// When the pointer (mouse cursor) enters the button field, 
    /// it shows the range for the upgrade version of the currently select item.
    /// </summary>
    /// <param name="eventData">Pointer Data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisButton.interactable)
        {
            if (GameManager.GetInstance().SelectedTile != null &&
               GameManager.GetInstance().SelectedTile.CurrentItem != null &&
               GameManager.GetInstance().SelectedTile.CurrentItem.GetComponent<Item>().UpgradeVersion != null)
            {
                _rangeGOUpgrade = GameManager.GetInstance().SelectedTile.CurrentItem.GetComponent<Item>().RangeGOUpgrade;
                _rangeGOUpgrade.SetActive(true);
            } 
        }
    }

    /// <summary>
    /// When the pointer (mouse cursor) exit the button field, 
    /// it hides the range for the upgrade version of the currently select item.
    /// </summary>
    /// <param name="eventData">Pointer Data</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide range upgrade.
        if (_rangeGOUpgrade != null)
        {
            _rangeGOUpgrade.SetActive(false);
        }
    }
}
