using System.Collections.Generic;
using UnityEngine;

public class AchievementRepository
{
    private const string SAVE_KEY = "AchievementSave";

    public void Save(List<AchievementDTO> dtos)
    {
        var json = JsonUtility.ToJson(new AchievementSaveWrapper { List = dtos }, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
    }

    public List<AchievementDTO> Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY)) return null;

        var json = PlayerPrefs.GetString(SAVE_KEY);
        return JsonUtility.FromJson<AchievementSaveWrapper>(json)?.List;
    }

    [System.Serializable]
    private class AchievementSaveWrapper
    {
        public List<AchievementDTO> List;
    }
}
