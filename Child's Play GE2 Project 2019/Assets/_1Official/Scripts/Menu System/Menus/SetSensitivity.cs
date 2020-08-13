using UnityEngine;
using UnityEngine.UI;



public class SetSensitivity : MonoBehaviour
{
    private enum SensitivityDirection { Horizontal, Vertical }
    private string _nameParam;
    public string NameParam { get { return _nameParam; } }

    private Button _applyButton;
    private float _prefValue;
    private Slider _slider;
    public Slider Slider { get { return _slider; } }
    [SerializeField] private Text _valueText;
    [SerializeField] private SensitivityDirection _sensDirection;

    /// <summary>
    /// Set the sensitivity base on the slider value
    /// </summary>
    /// <param name="sliderValue">Slider value</param>
    public void SetSens(float sliderValue)
    {
        if (_prefValue != _slider.value)
        {
            _applyButton.interactable = true;
        }
        _slider.value = sliderValue;
        _valueText.text = _slider.value.ToString("F2");
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        switch (_sensDirection)
        {
            case SensitivityDirection.Horizontal:
                _nameParam = Settings.GetInstance().SensitivityHParam;
                _prefValue = Settings.GetInstance().SensitivityH;
                break;
            case SensitivityDirection.Vertical:
                _nameParam = Settings.GetInstance().SensitivityVParam;
                _prefValue = Settings.GetInstance().SensitivityV;
                break;
            default:
                break;
        }
        
        _applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        _slider = GetComponent<Slider>();
        _slider.value = _prefValue;
    }

}
