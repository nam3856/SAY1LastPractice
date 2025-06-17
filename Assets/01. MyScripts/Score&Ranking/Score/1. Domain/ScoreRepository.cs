using System.Collections.Generic;
using UnityEngine;

public class ScoreRepository
{
    private const string SAVE_KEY = nameof(ScoreRepository);

    public void Save(ScoreDTO dto, string id)
    {
        // 변환
        var saveModels = new List<ScoreSaveModel>();

        var saveData = new ScoreSaveModel
        {
            Score = dto.Highscore,
            PlayerId = dto.PlayerId,
            ElapsedTime = dto.ElapsedPlayTime,
            IsCleared = dto.IsCleared
        };

        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(SAVE_KEY + "_" + id, json);
    }

    public ScoreDTO Load(string id)
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY + "_" + id))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY + "_" + id);
        var saveData = JsonUtility.FromJson<ScoreSaveModel>(json);
        if (saveData.PlayerId == null || saveData.Score <= 0 || saveData.ElapsedTime <= 0)
        {
            Debug.LogWarning("Invalid score data loaded: " + json);
            return null;
        }

        return new ScoreDTO(saveData);
    }
}
[System.Serializable]
public struct ScoreSaveModel
{
    public int Score;
    public string PlayerId;
    public float ElapsedTime;
    public bool IsCleared;
}
[System.Serializable]
public class ScoreSaveData
{
    public List<ScoreSaveModel> DataList;
}