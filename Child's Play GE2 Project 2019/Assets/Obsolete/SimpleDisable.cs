using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDisable : MonoBehaviour
{
    private Text[] _myButtonText;
    private Button _me;
    Color myColor = new Color(0.8823529f, 0.8980392f, 0.1411765f, 1f); //Yellow.

    // Start is called before the first frame update
    void Start()
    {
        _myButtonText = this.gameObject.GetComponentsInChildren<Text>();
        _me = this.gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_me.interactable)
        {
            foreach (var text in _myButtonText)
            {
                text.color = Color.gray;
            }
        }
        else
        {
            foreach (var text in _myButtonText)
            {
                text.color = myColor;
            }
        }
    }


}
