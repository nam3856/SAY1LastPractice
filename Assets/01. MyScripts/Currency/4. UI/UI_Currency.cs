using TMPro;
using Unity.FPS.Game;
using UnityEngine;

public class UI_Currency : MonoBehaviour
{
    public TextMeshProUGUI GoldCountText;
    public TextMeshProUGUI DiamondCountText;
    

    private void Start()
    {
        // 초기화
        Refresh();
        // 이벤트 구독
        CurrencyManager.Instance.CurrenciesChanged += Refresh;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.CurrenciesChanged -= Refresh;
        }
    }
    private void Refresh()
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
    }
}
