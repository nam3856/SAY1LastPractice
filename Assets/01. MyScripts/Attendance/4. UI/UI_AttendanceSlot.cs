using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttendanceSlot : MonoBehaviour
{
    public TextMeshProUGUI DayNameText;
    public GameObject[] CurrencyType;
    public TextMeshProUGUI CurrencyAmountText;
    public Toggle IsClaimedToggle;
    public GameObject HighlightEffect;

    private AttendanceRewardDTO _attendanceRewardDTO;

    public void Refresh(AttendanceRewardDTO attendanceRewardDTO)
    {
        _attendanceRewardDTO = attendanceRewardDTO;

        DayNameText.text = _attendanceRewardDTO.DayName;
        if (attendanceRewardDTO.RewardCurrencyType == ECurrencyType.Gold)
        {
            CurrencyType[0].SetActive(true);
            CurrencyType[1].SetActive(false);
        }
        else if (attendanceRewardDTO.RewardCurrencyType == ECurrencyType.Diamond)
        {
            CurrencyType[0].SetActive(false);
            CurrencyType[1].SetActive(true);
        }

        CurrencyAmountText.text = _attendanceRewardDTO.RewardCurrencyAmount.ToString();

        IsClaimedToggle.isOn = attendanceRewardDTO.IsClaimed;

        HighlightEffect?.SetActive(_attendanceRewardDTO.IsHighlight && !_attendanceRewardDTO.IsClaimed);
    }

}
