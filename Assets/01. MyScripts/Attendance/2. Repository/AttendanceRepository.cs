using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AttendanceRepository
{
    private const string ATTENDANCE_SAVE_KEY = "AttendanceDTO";
    private const string SLOT_SAVE_KEY = "AttendanceSlotDTOList";

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
    public void SaveSlots(List<AttendanceSlotDTO> slotDTOs)
    {
        AttendanceSlotSaveDataList saveDataList = new AttendanceSlotSaveDataList
        {
            DataList = slotDTOs.ConvertAll(dto => new AttendanceSlotSaveData
            {
                DayName = dto.DayName,
                RewardCurrencyType = dto.RewardCurrencyType,
                RewardCurrencyAmount = dto.RewardCurrencyAmount,
                IsClaimed = dto.IsClaimed
            })
        };
        string json = JsonUtility.ToJson(saveDataList);
        PlayerPrefs.SetString(SLOT_SAVE_KEY, json);
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

    public List<AttendanceSlotDTO> LoadSlots()
    {
        if (!PlayerPrefs.HasKey(SLOT_SAVE_KEY))
            return new List<AttendanceSlotDTO>();

        string json = PlayerPrefs.GetString(SLOT_SAVE_KEY);
        AttendanceSlotSaveDataList saveDataList = JsonUtility.FromJson<AttendanceSlotSaveDataList>(json);
        var result = new List<AttendanceSlotDTO>();
        if (saveDataList.DataList != null)
        {
            foreach (var data in saveDataList.DataList)
            {
                result.Add(new AttendanceSlotDTO(
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
            Slots = LoadSlots()
        };
    }

    [Serializable]
    public struct AttendanceSaveData
    {
        public int TotalAttendanceDays;
        public string LastAttendanceDate;
    }

    [Serializable]
    public struct AttendanceSlotSaveData
    {
        public string DayName;
        public ECurrencyType RewardCurrencyType;
        public int RewardCurrencyAmount;
        public bool IsClaimed;
    }

    [Serializable]
    public struct AttendanceSlotSaveDataList
    {
        public List<AttendanceSlotSaveData> DataList;
    }

    public struct AttendanceDataBundle
    {
        public AttendanceDTO AttendanceDTO;
        public List<AttendanceSlotDTO> Slots;
    }
}
