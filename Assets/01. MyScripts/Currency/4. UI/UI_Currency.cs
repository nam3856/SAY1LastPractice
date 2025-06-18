using TMPro;
using Unity.FPS.Game;
using UnityEngine;

public class UI_Currency : MonoBehaviour
{
    public TextMeshProUGUI GoldCountText;
    public TextMeshProUGUI DiamondCountText;
    public TextMeshProUGUI HealthUpgradeText;
    

    private void Start()
    {
        // 이벤트 구독
        GameManager.Instance.Events.Currency.OnCurrencyChanged += OnCurrencyChanged;

        InitManager.Instance.ReportInitialized("UI_Currency");
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (CurrencyManager.Instance != null)
        {
            GameManager.Instance.Events.Currency.OnCurrencyChanged -= OnCurrencyChanged;
        }
    }

    private void OnCurrencyChanged(CurrencyChangedEventArgs args)
    {
        switch (args.CurrencyType)
        {
            case ECurrencyType.Gold:
                GoldCountText.text = $"Gold: {args.NewValue}";
                break;
            case ECurrencyType.Diamond:
                DiamondCountText.text = $"Diamond: {args.NewValue}";
                break;
        }
        UpdateHealthBuyText();
    }

    private void UpdateHealthBuyText()
    {
        int gold = CurrencyManager.Instance.Currencies[ECurrencyType.Gold].Value;
        if (gold >= 500)
        {
            HealthUpgradeText.text = "Buy Health (500)";
            HealthUpgradeText.color = Color.green;
        }
        else
        {
            HealthUpgradeText.text = "Can't Buy Health (500)";
            HealthUpgradeText.color = Color.red;
        }
    }
    private void Initalize()
    {
        var dtoList = CurrencyManager.Instance.GetAllCurrencyDTOs();
        foreach (var dto in dtoList)
        {
            switch (dto.CurrencyType)
            {
                case ECurrencyType.Gold:
                    GoldCountText.text = $"{dto.CurrencyType.ToString()}: {dto.Value.ToString()}";
                    break;
                case ECurrencyType.Diamond:
                    DiamondCountText.text = $"{dto.CurrencyType.ToString()}: {dto.Value.ToString()}";
                    break;
            }
        }

        var goldDto = dtoList.Find(dto => dto.CurrencyType == ECurrencyType.Gold);

        if (goldDto != null && goldDto.HaveEnough(500))
        {
            HealthUpgradeText.text = "Buy Health (500)";
            HealthUpgradeText.color = Color.green;
        }
        else
        {
            HealthUpgradeText.text = "Can't Buy Health (500)";
            HealthUpgradeText.color = Color.red;
        }
    }
}
