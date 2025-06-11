using System;
using System.Collections.Generic;
using UnityEngine;
using static MockAttendanceRepository;

public class AttendanceManager : MonoBehaviour
{
    [SerializeField] private List<AttendanceDataSO> _attendanceDataList;
    private List<AttendanceSlot> _attendanceSlots;
    public IReadOnlyList<AttendanceSlot> AttendanceSlots => _attendanceSlots;

    private Attendance _attendance;

    public static AttendanceManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(AttendanceDTO savedData = null)
    {
        if (savedData != null)
        {
            _attendance = new Attendance(savedData.TotalAttendanceDays, savedData.LastAttendanceDate);
        }
        else
        {
            _attendance = new Attendance(0, DateTime.MinValue); // 첫 출석
        }

        // Attendance Slot 초기화
        CreateSlots();
    }

    private void CreateSlots()
    {
        _attendanceSlots = new List<AttendanceSlot>();
        foreach (var data in _attendanceDataList)
        {
            string dayName = $"{data.Day}일차";
            var slot = new AttendanceSlot(dayName, data.RewardCurrencyType, data.RewardCurrencyAmount);
            _attendanceSlots.Add(slot);
        }
    }
    public AttendanceSlot GetTodaySlot()
    {
        int index = Mathf.Clamp(_attendance.TotalAttendanceDays, 0, _attendanceSlots.Count - 1);
        return _attendanceSlots[index];
    }


    /// <summary>
    /// 접속시 오늘 출석을 체크하고, 출석을 완료하면 출석 일수를 증가시킵니다.
    /// </summary>
    public void CheckTodayAttendance()
    {
        if (_attendance.CheckTodayAttendance())
        {
            _attendance.Increase(1);
            Debug.Log("오늘 출석을 완료했어요!");
        }
        else
        {
            Debug.Log("오늘은 이미 출석을 했어요.");
        }
    }

    /// <summary>
    /// 받을 수 있는 모든 출석 보상을 수령합니다.
    /// </summary>
    public void ClaimAllAvailableRewards()
    {
        if (_attendanceSlots == null || _attendanceSlots.Count == 0)
        {
            Debug.LogWarning("슬롯이 초기화되지 않았어용~");
            return;
        }

        int claimableCount = Mathf.Min(_attendance.TotalAttendanceDays, _attendanceSlots.Count);

        for (int i = 0; i < claimableCount; i++)
        {
            var slot = _attendanceSlots[i];
            if (slot.TryClaim())
            {
                GrantReward(slot);
            }
        }
    }

    private void GrantReward(AttendanceSlot slot)
    {
        GameManager.Instance.Events.Attendance.RaiseRewardClaimed(new AttendanceSlotDTO(slot));
        Debug.Log($"보상 수령해용~ {slot.DayName}, {slot.RewardCurrencyType}, {slot.RewardCurrencyAmount}");
    }

    /// <summary>
    /// 출석 슬롯 DTO 리스트를 반환합니다.
    /// AttendanceSlotDTO.IsHighLight 속성을 이용하여 출석했지만 안받은 보상들을 하이라이트 시킬 수 있습니다. 
    /// </summary>
    /// <returns></returns>
    public List<AttendanceSlotDTO> GetAttendanceSlotDTOs()
    {
        var list = new List<AttendanceSlotDTO>();
        for (int i = 0; i < _attendanceSlots.Count; i++)
        {
            var slot = _attendanceSlots[i];
            list.Add(new AttendanceSlotDTO(slot, _attendance.TotalAttendanceDays, i));
        }
        return list;
    }

    public AttendanceDTO GetCurrentAttendanceDTO()
    {
        return new AttendanceDTO(_attendance);
    }

    public void LoadClaimStates(List<AttendanceSlotDTO> dtos)
    {
        for (int i = 0; i < dtos.Count && i < _attendanceSlots.Count; i++)
        {
            _attendanceSlots[i].IsClaimed = dtos[i].IsClaimed;
        }
    }

    public MockAttendanceSaveModel ToSaveModel()
    {
        return new MockAttendanceSaveModel
        {
            AttendanceData = GetCurrentAttendanceDTO(),
            SlotDataList = GetAttendanceSlotDTOs()
        };
    }

    public void LoadFromSaveModel(MockAttendanceSaveModel model)
    {
        Initialize(model.AttendanceData);
        LoadClaimStates(model.SlotDataList);
    }
}