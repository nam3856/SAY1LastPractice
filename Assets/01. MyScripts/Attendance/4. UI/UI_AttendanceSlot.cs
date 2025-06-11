using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttendanceSlot : MonoBehaviour
{
    public TextMeshProUGUI DayNameText;
    public GameObject[] CurrencyType;
    public TextMeshProUGUI CurrencyAmountText;
    public Toggle IsClaimedToggle;

    private AttendanceSlotDTO _attendanceSlotDTO;

    public void Refresh(AttendanceSlotDTO attendanceSlotDTO)
    {
        _attendanceSlotDTO = attendanceSlotDTO;

        DayNameText.text = _attendanceSlotDTO.DayName;
        if (attendanceSlotDTO.RewardCurrencyType == ECurrencyType.Gold)
        {
            CurrencyType[0].SetActive(true);
            CurrencyType[1].SetActive(false);
        }
        else if (attendanceSlotDTO.RewardCurrencyType == ECurrencyType.Diamond)
        {
            CurrencyType[0].SetActive(false);
            CurrencyType[1].SetActive(true);
        }

        CurrencyAmountText.text = _attendanceSlotDTO.RewardCurrencyAmount.ToString();

        IsClaimedToggle.isOn = attendanceSlotDTO.IsClaimed;

    }

}
