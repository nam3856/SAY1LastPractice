using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementRepository
{
    private const string SAVE_KEY = "AchievementSave";

    public void Save(List<AchievementDTO> dtos)
    {
        var saveModels = new List<AchievementSaveModel>();
        foreach (var dto in dtos)
        {
            saveModels.Add(dto.ToSaveModel());
        }

        var wrapper = new AchievementSaveWrapper { List = saveModels };
        var json = JsonUtility.ToJson(wrapper, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
    }

    public List<AchievementDTO> Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        var saveData = JsonUtility.FromJson<AchievementSaveWrapper>(json);
        if (saveData?.List == null)
            return null;

        var dtos = new List<AchievementDTO>();
        foreach (var model in saveData.List)
        {
            dtos.Add(new AchievementDTO(model));
        }

        return dtos;
    }

    [System.Serializable]
    public struct AchievementSaveModel
    {
        public string ID;
        public string Name;
        public string Description;
        public EAchievementCondition Condition;
        public int GoalValue;
        public int CurrentValue;
        public bool Rewarded;
        public ECurrencyType RewardCurrencyType;
        public int RewardCurrencyAmount;
        public string ClaimedDate;
        public bool IsUnlocked;
    }

    [System.Serializable]
    private class AchievementSaveWrapper
    {
        public List<AchievementSaveModel> List;
    }
}
