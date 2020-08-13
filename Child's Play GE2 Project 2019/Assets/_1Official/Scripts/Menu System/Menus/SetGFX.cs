using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SetGFX : MonoBehaviour {

    [SerializeField] private Text _qualityText;
    private Button _applyButton;
    private string[] _gfxNames;
    private float _prefValue;
    public float PrefValue { get { return _prefValue; } set { _prefValue = value; } }
    private Slider _slider;
    public Slider Slider { get { return _slider; } }

    /// <summary>
    /// Set the game quality base on the slider value
    /// </summary>
    /// <param name="sliderValue">the slider value</param>
    public void SetQuality(float sliderValue)
    {
        int gfxIndex = (int)Mathf.Floor(sliderValue);
        _qualityText.text = _gfxNames[gfxIndex];
        if (_prefValue != _slider.value)
        {
            _applyButton.interactable = true;
        }
        _slider.value = sliderValue;
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start () {
        _applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        _slider = GetComponent<Slider>();
        _gfxNames = QualitySettings.names;
        _slider.value = _prefValue;
    }

}
