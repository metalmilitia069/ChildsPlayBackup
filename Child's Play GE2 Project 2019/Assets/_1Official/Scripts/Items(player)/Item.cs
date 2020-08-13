using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Normal,
    Placeholder
}

public class Item : Player
{
    [Header("Item Option")]
    [SerializeField] private ItemType _itemType = ItemType.Normal;
    [SerializeField] private string itemName;
    [SerializeField, Multiline] private string itemDescription;
    /// <summary>
    /// this is the base value of the Item, when the players buys this item the value is reduce 
    /// but it will go up as this item is upgraded in order to increase its resell value.
    /// </summary>
    [SerializeField] private int value = 1;
    [SerializeField] private int indexInGM;
    [SerializeField] private GameObject rangeGO;
    [SerializeField] private GameObject rangeGOUpgrade;
    [SerializeField] private Item upgradeVersion;

    public GameObject RangeGO { get => rangeGO; }
    public int Value { get => value; set => this.value = value; }
    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    public int IndexInGM { get => indexInGM; }
    public Item UpgradeVersion { get => upgradeVersion; }
    public GameObject RangeGOUpgrade { get => rangeGOUpgrade; set => rangeGOUpgrade = value; }

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    private void Awake()
    {
        if (rangeGO != null)
        {
            SetRangeScale(GetComponent<Tower>().Range);
        }
        if (rangeGOUpgrade != null)
        {
            rangeGOUpgrade.SetActive(false);
            if (upgradeVersion != null)
            {
                SetRangeUPScale(upgradeVersion.GetComponent<Tower>().Range);
            }
        }
        if (_itemType == ItemType.Placeholder)
        {
            Destroy(GetComponent<Tower>());
            Destroy(this);
        }
        else
        {
            value /= 2;
            PlayerManager.GetInstance().AddPlayer(this);
        }
    }

    /// <summary>
    /// Used to scale the Range of the Item to the size set.
    /// </summary>
    /// <param name="scaleV3">The size of the Range</param>
    public void SetRangeScale(float scaleV3)
    {
        rangeGO.transform.localScale = Vector3.one * scaleV3 * 2.0f;
        rangeGO.transform.localScale = new Vector3(rangeGO.transform.localScale.x,
            Mathf.Clamp(rangeGO.transform.localScale.y * 0.33f, 0.0f, 10.0f), rangeGO.transform.localScale.z);
    }

    /// <summary>
    /// Used to scale the Range of the Item Upgraded version to the size set.
    /// </summary>
    /// <param name="scaleV3">The size of the Range</param>
    public void SetRangeUPScale(float scaleV3)
    {
        rangeGOUpgrade.transform.localScale = Vector3.one * scaleV3 * 2.0f;
        rangeGOUpgrade.transform.localScale = new Vector3(rangeGOUpgrade.transform.localScale.x,
            Mathf.Clamp(rangeGOUpgrade.transform.localScale.y * 0.33f, 0.0f, 10.0f), rangeGOUpgrade.transform.localScale.z);
    }

}