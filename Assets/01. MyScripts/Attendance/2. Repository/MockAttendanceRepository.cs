using System.Collections.Generic;
using UnityEngine;

public class MockAttendanceRepository
{
    private const string SAVE_KEY = "Attendance";

    public void Save(MockAttendanceSaveModel saveModel, string id)
    {
        var wrapper = new MockAttendanceSaveWrapper
        {
            List = new List<MockAttendanceSaveModel> { saveModel }
        };

        string json = JsonUtility.ToJson(wrapper, true);
        PlayerPrefs.SetString(SAVE_KEY + "_" + id, json);
    }

    public MockAttendanceSaveModel? Load(string id)
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY + "_" + id))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY + "_" + id);
        var wrapper = JsonUtility.FromJson<MockAttendanceSaveWrapper>(json);
        if (wrapper?.List == null || wrapper.List.Count == 0)
            return null;

        return wrapper.List[0];
    }

    [System.Serializable]
    public struct MockAttendanceSaveModel
    {
        public AttendanceDTO AttendanceData;
        public List<AttendanceSlotDTO> SlotDataList;

    }

    [System.Serializable]
    private class MockAttendanceSaveWrapper
    {
        public List<MockAttendanceSaveModel> List;
    }
}