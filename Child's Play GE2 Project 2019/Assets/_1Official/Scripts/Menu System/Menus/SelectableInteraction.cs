///
/// Author: Alexandre Lepage
/// Date: October 2018
/// Desc: Project for LaSalle College
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableInteraction : MonoBehaviour,IPointerEnterHandler,IDeselectHandler,ISelectHandler {

	private bool _selected = false;
	public bool Selected { get { return _selected; } }

    /// <summary>
    /// When cursor enters above the object
    /// </summary>
    /// <param name="eventData">Unity data</param>
	public void OnPointerEnter(PointerEventData eventData)
	{
		GetComponent<Selectable>().Select();
        SoundManager.GetInstance().PlaySoundOneShot(Sound.OnButtonOver,0.5f);
    }

    /// <summary>
    /// When the object is deselected
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Selectable>().OnPointerExit(null);
        _selected = false;
    }

    /// <summary>
    /// When the object is selected
    /// </summary>
    /// <param name="eventData"></param>
	public void OnSelect(BaseEventData eventData)
	{
		_selected = true;
	}
}   
