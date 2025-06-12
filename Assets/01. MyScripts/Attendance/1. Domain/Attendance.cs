using System;
using System.Collections.Generic;
using UnityEngine;

public class Attendance
{
    public int TotalAttendanceDays { get; private set; }
    public DateTime LastAttendanceDate { get; private set; }
    private List<AttendanceReward> _rewards;
    public IReadOnlyList<AttendanceReward> Rewards => _rewards;
    public Attendance(int totalAttendanceDays, DateTime lastAttendanceDate, List<AttendanceReward> rewards)
    {
        if(totalAttendanceDays < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(totalAttendanceDays), "총 출석 일수는 0 이상이어야 합니다.");
        }
        if(lastAttendanceDate < DateTime.MinValue)
        {
            throw new System.ArgumentOutOfRangeException(nameof(lastAttendanceDate), "마지막 출석 날짜는 최소값 이상이어야 합니다.");
        }

        TotalAttendanceDays = totalAttendanceDays;
        LastAttendanceDate = lastAttendanceDate;

        TotalAttendanceDays = totalAttendanceDays;
        LastAttendanceDate = lastAttendanceDate;
        _rewards = rewards ?? throw new ArgumentNullException(nameof(rewards));
    }

    public bool CheckTodayAttendance() => LastAttendanceDate.Date < DateTime.Now.Date;

    public bool RecordAttendance()
    {
        if (!CheckTodayAttendance())
            return false;

        if (TotalAttendanceDays >= _rewards.Count)
            ResetCycle();

        TotalAttendanceDays++;
        LastAttendanceDate = DateTime.Now;
        return true;
    }

    public void ResetCycle()
    {
        TotalAttendanceDays = 0;
        foreach (var reward in _rewards)
            reward.ResetClaim();
    }

    public List<AttendanceReward> ClaimAvailableRewards()
    {
        var claimable = new List<AttendanceReward>();
        for (int i = 0; i < Math.Min(TotalAttendanceDays, _rewards.Count); i++)
        {
            if (_rewards[i].MarkAsClaimedIfPossible())
                claimable.Add(_rewards[i]);
        }
        return claimable;
    }

    public static Attendance CreateNew(List<AttendanceReward> rewards)
    {
        return new Attendance(0, DateTime.MinValue, rewards);
    }

    public static Attendance CreateFromDTO(AttendanceDTO dto, List<AttendanceReward> rewards)
    {
        return new Attendance(dto.TotalAttendanceDays, dto.LastAttendanceDate, rewards);
    }
    public void LoadClaimStates(List<AttendanceRewardDTO> dtos)
    {
        for (int i = 0; i < dtos.Count && i < _rewards.Count; i++)
        {
            if (dtos[i].IsClaimed)
                _rewards[i].MarkAsClaimedIfPossible();
            else
                _rewards[i].ResetClaim();
        }
    }

    public AttendanceDTO ToDTO()
    {
        return new AttendanceDTO(this);
    }

    public bool HasUnclaimedReward()
    {
        int claimableCount = Math.Min(TotalAttendanceDays, _rewards.Count);
        for (int i = 0; i < claimableCount; i++)
        {
            if (_rewards[i].CanClaimReward())
                return true;
        }
        return false;
    }
}
