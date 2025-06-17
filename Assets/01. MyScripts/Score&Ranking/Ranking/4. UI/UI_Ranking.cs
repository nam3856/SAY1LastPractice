using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager.Instance.Events.Ranking.OnRankingUpdated 이벤트를 구독하여 랭킹 UI를 업데이트하는 MonoBehaviour입니다.
/// RankingManager.Instance.GetTopRankings() 로 랭킹 리스트를 받아올 수 있습니다.
/// RankingManager.Instance.GetMyRanking(string myPlayerId)로 내 랭킹을 받아올 수 있습니다.
/// </summary>
public class UI_Ranking : MonoBehaviour
{
    [SerializeField] private GameObject rankingPrefab;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private List<UI_RankingEntry> entryUIList = new List<UI_RankingEntry>();
    [SerializeField] private UI_RankingEntry myScoreUI;

    private void Start()
    {
        GameManager.Instance.Events.Ranking.OnRankingUpdated += OnRankingUpdated;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Events.Ranking.OnRankingUpdated -= OnRankingUpdated;
        }
    }

    private void OnRankingUpdated()
    {
        InitEmptyRanking();
        //RankingManager.Instance.OnRankingUpdated += UpdateRanking;
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
