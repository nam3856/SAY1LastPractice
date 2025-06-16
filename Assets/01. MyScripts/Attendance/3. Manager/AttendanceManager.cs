using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AttendanceManager : MonoBehaviour
{
    [SerializeField] private List<AttendanceDataSO> _attendanceDataList;

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
        var rewards = CreateRewards();

        _attendance = savedData != null
            ? Attendance.CreateFromDTO(savedData, rewards)
            : Attendance.CreateNew(rewards);

        GameManager.Instance.Events.Attendance.RaiseAttendanceInitialized();
        InitManager.Instance.ReportInitialized("Attendance");
    }

    private List<AttendanceReward> CreateRewards()
    {
        var rewardList = new List<AttendanceReward>();
        foreach (var data in _attendanceDataList)
        {
            string dayName = $"{data.Day}일차";
            var reward = new AttendanceReward(dayName, data.RewardCurrencyType, data.RewardCurrencyAmount);
            rewardList.Add(reward);
        }
        return rewardList;
    }
    public void CheckTodayAttendance()
    {
        if (_attendance.RecordAttendance())
        {
            GameManager.Instance.Events.Attendance.RaiseTodayAttendanceChecked(true);
            GameManager.Instance.SaveRequested();
        }
        else
        {
            GameManager.Instance.Events.Attendance.RaiseTodayAttendanceChecked(false);
        }
    }
    public void ClaimAllAvailableRewards()
    {
        var claimed = _attendance.ClaimAvailableRewards();
        foreach (var reward in claimed)
            GrantReward(reward);
    }

    private void GrantReward(AttendanceReward reward)
    {
        GameManager.Instance.Events.Attendance.RaiseRewardClaimed(new AttendanceRewardDTO(reward, false));
        Debug.Log($"보상 수령해용~ {reward.DayName}, {reward.RewardCurrencyType}, {reward.RewardCurrencyAmount}");
    }

    public bool HasUnclaimedRewardAvailable()
    {
        return _attendance.HasUnclaimedReward();
    }


    /// <summary>
    /// 출석 슬롯 DTO 리스트를 반환합니다.
    /// AttendanceRewardDTO.IsHighLight 속성을 이용하여 출석했지만 안받은 보상들을 하이라이트 시킬 수 있습니다. 
    /// </summary>
    /// <returns></returns>
    public List<AttendanceRewardDTO> GetAttendanceSlotDTOs()
    {
        var list = new List<AttendanceRewardDTO>();
        for (int i = 0; i < _attendance.Rewards.Count; i++)
        {
            var reward = _attendance.Rewards[i];
            bool isHighlight = i < _attendance.TotalAttendanceDays && !reward.IsClaimed;
            list.Add(new AttendanceRewardDTO(reward, isHighlight));
        }
        return list;
    }

    public AttendanceDTO GetCurrentAttendanceDTO()
    {
        return _attendance.ToDTO();
    }

    public void LoadClaimStates(List<AttendanceRewardDTO> dtos)
    {
        _attendance.LoadClaimStates(dtos);
    }


    public void LoadFromSaveModel(List<AttendanceRewardDTO> slotDTOs, AttendanceDTO attendance)
    {
        Initialize(attendance);
        LoadClaimStates(slotDTOs);
    }
}