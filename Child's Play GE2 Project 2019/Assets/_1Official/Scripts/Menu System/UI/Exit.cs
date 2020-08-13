using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Exit : MonoBehaviour, IPointerEnterHandler
{
    /// <summary>
    /// When cursor enters above the object
    /// </summary>
    /// <param name="eventData">Unity data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.GetInstance().DeselectTile();
    }
}
