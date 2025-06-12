using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attendance : MonoBehaviour
{
    public Transform Parent;
    public GameObject SlotPrefab;
    private List<UI_AttendanceSlot> _slots;
    public Button GetButton;

    private void Start()
    {
        GameManager.Instance.Events.Attendance.OnAttendanceInitialized += Init;
        GameManager.Instance.Events.Attendance.OnTodayAttendanceChecked += Refresh;

    }

    public void Init()
    {
        List<AttendanceRewardDTO> slotDTOs = AttendanceManager.Instance.GetAttendanceSlotDTOs();
        _slots = new List<UI_AttendanceSlot>();

        for (int i = 0; i < slotDTOs.Count; i++)
        {
            GameObject slotObject = Instantiate(SlotPrefab, Parent);
            _slots.Add(slotObject.GetComponent<UI_AttendanceSlot>());
        }
        Refresh();

        InitManager.Instance.ReportInitialized("UI_Attendance");
    }


    public void Refresh(bool _ = false)
    {
        List<AttendanceRewardDTO> slotDTOs = AttendanceManager.Instance.GetAttendanceSlotDTOs();

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


