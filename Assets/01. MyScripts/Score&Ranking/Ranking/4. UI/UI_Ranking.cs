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
        InitEmptyRanking();
        GameManager.Instance.Events.Ranking.OnRankingUpdated += OnRankingUpdated;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Events.Ranking.OnRankingUpdated -= OnRankingUpdated;
        }
    }

    //private void OnRankingUpdated()
    //{
    //    var rankingList = RankingManager.Instance.GetTopRankings();
    //    var myRank = RankingManager.Instance.GetMyRanking(GameManager.Instance.PlayerId);

    //    // 변환: RankingEntry → RankingDataSO (또는 임시 구조체 사용)
    //    var rankingDataList = new List<RankingDataSO>();
    //    for (int i = 0; i < rankingList.Count; i++)
    //    {
    //        var entry = rankingList[i];
    //        rankingDataList.Add(new RankingDataSO
    //        {
    //            Rank = i + 1,
    //            PlayerName = entry.PlayerName,
    //            Score = entry.Score
    //        });
    //    }

    //    var myEntry = rankingList.FirstOrDefault(e => e.PlayerId == GameManager.Instance.PlayerId);
    //    var myData = new RankingDataSO
    //    {
    //        Rank = myRank ?? -1,
    //        PlayerName = myEntry?.PlayerName ?? "Unknown",
    //        Score = myEntry?.Score ?? 0
    //    };

    //    UpdateRanking(rankingDataList, myData);
    //}

    private void OnRankingUpdated()
    {
        var rankingList = RankingManager.Instance.GetTopRankings();
        var myRank = RankingManager.Instance.GetMyRanking("");

        // 랭킹 리스트와 내 랭킹을 가져옵니다.
        for (int i = 0; i < entryUIList.Count; i++)
        {
            entryUIList[i].SetData((i + 1).ToString(), rankingList[i].Nickname, rankingList[i].Score.ToString());
        }

        if (myRank.HasValue)
        {

        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }


    private void InitEmptyRanking()
    {
        if (entryUIList.Count > 0)
            return; // 이미 초기화됨

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
