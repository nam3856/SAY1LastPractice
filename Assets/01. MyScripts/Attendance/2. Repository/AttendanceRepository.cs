using System;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceRepository
{
    private const string ATTENDANCE_SAVE_KEY = "AttendanceDTO";
    private const string REWARD_SAVE_KEY = "AttendanceRewardDTOList";

    // 출석 정보 저장
    public void SaveAttendance(AttendanceDTO attendanceDTO)
    {
        AttendanceSaveData saveData = new AttendanceSaveData
        {
            TotalAttendanceDays = attendanceDTO.TotalAttendanceDays,
            LastAttendanceDate = attendanceDTO.LastAttendanceDate.ToString("o")
        };
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(ATTENDANCE_SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    // 슬롯 정보 저장
    public void SaveRewards(List<AttendanceRewardDTO> rewardDTOs)
    {
        AttendanceRewardSaveDataList saveDataList = new AttendanceRewardSaveDataList
        {
            DataList = rewardDTOs.ConvertAll(dto => new AttendanceRewardSaveData
            {
                DayName = dto.DayName,
                RewardCurrencyType = dto.RewardCurrencyType,
                RewardCurrencyAmount = dto.RewardCurrencyAmount,
                IsClaimed = dto.IsClaimed
            })
        };
        string json = JsonUtility.ToJson(saveDataList);
        PlayerPrefs.SetString(REWARD_SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    // 출석 정보 로드
    public AttendanceDTO LoadAttendance()
    {
        if (!PlayerPrefs.HasKey(ATTENDANCE_SAVE_KEY))
            return null;

        string json = PlayerPrefs.GetString(ATTENDANCE_SAVE_KEY);
        AttendanceSaveData saveData = JsonUtility.FromJson<AttendanceSaveData>(json);
        DateTime savedDateTime = string.IsNullOrEmpty(saveData.LastAttendanceDate) ? DateTime.MinValue : DateTime.Parse(saveData.LastAttendanceDate);
        return new AttendanceDTO(saveData.TotalAttendanceDays, savedDateTime);
    }

    public List<AttendanceRewardDTO> LoadRewards()
    {
        if (!PlayerPrefs.HasKey(REWARD_SAVE_KEY))
            return new List<AttendanceRewardDTO>();

        string json = PlayerPrefs.GetString(REWARD_SAVE_KEY);
        AttendanceRewardSaveDataList saveDataList = JsonUtility.FromJson<AttendanceRewardSaveDataList>(json);
        var result = new List<AttendanceRewardDTO>();
        if (saveDataList.DataList != null)
        {
            foreach (var data in saveDataList.DataList)
            {
                result.Add(new AttendanceRewardDTO(
                    data.DayName,
                    data.RewardCurrencyType,
                    data.RewardCurrencyAmount,
                    data.IsClaimed
                ));
            }
        }
        return result;
    }

    // 출석 정보와 슬롯 정보를 한 번에 반환
    public AttendanceDataBundle LoadAll()
    {
        return new AttendanceDataBundle
        {
            AttendanceDTO = LoadAttendance(),
            Rewards = LoadRewards()
        };
    }

    [Serializable]
    public struct AttendanceSaveData
    {
        public int TotalAttendanceDays;
        public string LastAttendanceDate;
    }

    [Serializable]
    public struct AttendanceRewardSaveData
    {
        public string DayName;
        public ECurrencyType RewardCurrencyType;
        public int RewardCurrencyAmount;
        public bool IsClaimed;
    }

    [Serializable]
    public struct AttendanceRewardSaveDataList
    {
        public List<AttendanceRewardSaveData> DataList;
    }

    public struct AttendanceDataBundle
    {
        public AttendanceDTO AttendanceDTO;
        public List<AttendanceRewardDTO> Rewards;
    }
}
