using System.Collections.Generic;
using UnityEngine;

public class UI_Ranking : MonoBehaviour
{
    [SerializeField] private GameObject rankingPrefab;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private List<UI_RankingEntry> entryUIList = new List<UI_RankingEntry>();
    [SerializeField] private UI_RankingEntry myScoreUI;

    private void Start()
    {
        InitEmptyRanking();
    }

    private void InitEmptyRanking()
    {
        for (int i = 0; i < 10; i++)
        {
            var entryGO = Instantiate(rankingPrefab, entryContainer);
            var entryUI = entryGO.GetComponent<UI_RankingEntry>();
            entryUI.SetData("-", "-", "-");
            entryUIList.Add(entryUI);
        }
    }

    public void UpdateRanking(List<RankingDataSO> rankingList, RankingDataSO myData)
    {
        for (int i = 0; i < entryUIList.Count; i++)
        {
            if (i < rankingList.Count)
            {
                var data = rankingList[i];
                entryUIList[i].SetData(data.Rank.ToString(), data.PlayerName, data.Score.ToString());
            }
            else
            {
                entryUIList[i].SetData("-", "-", "-");
            }
        }
        myScoreUI.SetData("-", myData.PlayerName, myData.Score.ToString());
    }
}
