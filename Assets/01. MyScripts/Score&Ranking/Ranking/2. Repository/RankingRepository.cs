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
        return new List<RankingEntry>()
        {
            new RankingEntry(181, "test1@test.com", "냉철한토끼65", true, 124.12f),
            new RankingEntry(271, "test2@test.com", "빛나는호랑이922", false, 42.22f),
            new RankingEntry(260, "test3@test.com", "달콤한햄스터489", true, 551.24f),
            new RankingEntry(263, "test4@test.com", "따뜻한토끼754", true, 230.5f),
            new RankingEntry(152, "test5@test.com", "귀여운여우451", false,25.12f),
            new RankingEntry(206, "test6@test.com", "행복한고양이621", true,66666f),
            new RankingEntry(205, "test7@test.com", "따뜻한햄스터558", false,42212f),
            new RankingEntry(111, "test8@test.com", "우주고양이980", false,12f),
            new RankingEntry(219, "test9@test.com", "무서운사자570", false,5242f),
            new RankingEntry(149, "test10@test.com", "우주토끼732",false,2512109f )
        };



        //if (!PlayerPrefs.HasKey(SAVE_KEY))
        //    return new List<RankingEntry>();

        //string json = PlayerPrefs.GetString(SAVE_KEY);
        //return JsonUtility.FromJson<RankingListWrapper>(json).List;
    }

    [System.Serializable]
    private class RankingListWrapper
    {
        public List<RankingEntry> List;
    }
}