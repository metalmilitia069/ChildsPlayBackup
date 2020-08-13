using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudComponent : MonoBehaviour
{
    [SerializeField] private Food _food;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private Text _moneyTxt;
    [SerializeField] private Text _foodPercentageTxt;
    [SerializeField] private Text _warmUpText;
    private float foodRemaining;

    public Text MoneyTxt { get => _moneyTxt; set => _moneyTxt = value; }
    public Text FoodPercentageTxt { get => _foodPercentageTxt; set => _foodPercentageTxt = value; }
    public float FoodRemaining { get => foodRemaining; set => foodRemaining = value; }
    public Text WarmUpText { get => _warmUpText; set => _warmUpText = value; }
    public Image FillerImage { get => _fillerImage; set => _fillerImage = value; }

}
