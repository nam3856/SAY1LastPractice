using UnityEngine;

/// <summary>
/// GameManager.Instance.Events.Ranking.OnRankingUpdated 이벤트를 구독하여 랭킹 UI를 업데이트하는 MonoBehaviour입니다.
/// RankingManager.Instance.GetTopRankings() 로 랭킹 리스트를 받아올 수 있습니다.
/// RankingManager.Instance.GetMyRanking(string myPlayerId)로 내 랭킹을 받아올 수 있습니다.
/// </summary>
public class UI_Ranking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
