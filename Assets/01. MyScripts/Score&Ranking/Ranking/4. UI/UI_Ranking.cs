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

    private void OnRankingUpdated()
    {
        var rankingList = RankingManager.Instance.GetTopRankings();
        var playerID = ScoreManager.Instance.PlayerId;
        var myRank = RankingManager.Instance.GetMyRanking(playerID);

        // 랭킹 리스트와 내 랭킹을 가져옵니다.
        for (int i = 0; i < entryUIList.Count; i++)
        {
            //entryUIList[i].SetData((i + 1).ToString(), rankingList[i].Nickname, rankingList[i].Score.ToString());
            if (i < rankingList.Count)
            {
                var entry = rankingList[i];
                entryUIList[i].SetData((i + 1).ToString(), entry.Nickname, entry.Score.ToString());
            }
            else
            {
                entryUIList[i].SetData("-", "-", "-");
            }

            if (myRank.HasValue)
            {
                var myEntry = rankingList.Find(e => e.PlayerId == playerID);
                if (myEntry != null)
                {
                    myScoreUI.SetData(myRank.Value.ToString(), myEntry.Nickname, myEntry.Score.ToString());
                }
                else
                {
                    var nickname = AccountManager.Instance?.GetNicknameByPlayerId(playerID) ?? "Unknown";
                    var score = ScoreManager.Instance.CurrentScore?.Highscore ?? 0;
                    myScoreUI.SetData(myRank.Value.ToString(), nickname, score.ToString());
                }
            }
            else
            {
                myScoreUI.SetData("-", "-", "-");
            }

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
