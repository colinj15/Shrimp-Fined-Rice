using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaxController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taxText;
    [SerializeField] private RectTransform taxBar;
    private float maxTaX = 76.83f;
    private float minTaX = 826.3f;
    private float currentTax = 0f;
    public TMP_InputField myInputField;

    void Start()
    {
        myInputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
    }
    public void setPercentage()
    {
        int percent = Mathf.RoundToInt(int.Parse(myInputField.text) / 100f * 200f);
        float value = minTaX + (maxTaX - minTaX) * (percent / 200f);
        taxBar.sizeDelta = new Vector2(value, taxBar.sizeDelta.y);
        currentTax = percent;
    }
}