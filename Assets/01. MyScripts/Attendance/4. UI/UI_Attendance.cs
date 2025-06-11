using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attendance : MonoBehaviour
{
    private List<UI_AttendanceSlot> _slots;
    public Button GetButton;

    private void Start()
    {
        GameManager.Instance.Events.Attendance.OnAttendanceInitialized += Refresh;

        InitManager.Instance.ReportInitialized("UI_Attendance");
    }

    public void Refresh()
    {
        List<AttendanceSlotDTO> slotDTOs = AttendanceManager.Instance.GetAttendanceSlotDTOs();

        for (int i = 0; i < slotDTOs.Count; i++)
        {
            _slots[i].Refresh(slotDTOs[i]);
        }
        GetButton.interactable = AttendanceManager.Instance.HasUnclaimedRewardAvailable();
    }

    public void ClaimReward()
    {
        if (true)
        {
            AttendanceManager.Instance.ClaimAllAvailableRewards();
        }
        Refresh();
    }


}


