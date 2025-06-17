using System.Collections.Generic;
using UnityEngine;

public class RankingRepository
{
    private const string SAVE_KEY = "RANKING_LIST";

    public void Save(List<RankingEntry> rankingList)
    {
        var json = JsonUtility.ToJson(new RankingListWrapper { List = rankingList });
        PlayerPrefs.SetString(SAVE_KEY, json);
    }

    public List<RankingEntry> Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return new List<RankingEntry>();

        string json = PlayerPrefs.GetString(SAVE_KEY);
        return JsonUtility.FromJson<RankingListWrapper>(json).List;
    }

    [System.Serializable]
    private class RankingListWrapper
    {
        public List<RankingEntry> List;
    }
}