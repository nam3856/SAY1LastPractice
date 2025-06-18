using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }
    private List<RankingEntry> _rankingList;

    public int MaxRankCount = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(List<RankingEntry> rankingList = null)
    {
        if (rankingList == null)
        {
            _rankingList = new List<RankingEntry>();
        }
        else
        {
            _rankingList = rankingList;
        }

        GameManager.Instance.Events.Score.OnHighScoreUpdated += HandleHighScoreUpdated;
        GameManager.Instance.Events.Score.OnScoreCalculateFinished += OnRankingSortComplete;
        
        //초기 정렬
        _rankingList = _rankingList
            .OrderByDescending(e => e.Score)
            .ThenByDescending(e => e.IsCleared)
            .ThenBy(e => e.ElapsedPlayTime)
            .Take(MaxRankCount)
            .ToList();

    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Events.Score.OnHighScoreUpdated -= HandleHighScoreUpdated;
            GameManager.Instance.Events.Score.OnScoreCalculateFinished -= OnRankingSortComplete;
        }
    }

    private void OnRankingSortComplete(ScoreDTO scoreDTO)
    {
        GameManager.Instance.Events.Ranking.RaiseRankingUpdated();
    }
    private void HandleHighScoreUpdated(ScoreDTO scoreDTO)
    {
        if (scoreDTO == null)
        {
            Debug.LogWarning("ScoreDTO is null. Cannot process Add Ranking.");
            return;
        }
        // 랭킹에 추가 시도
        TryAddRanking(scoreDTO);
    }
    public void TryAddRanking(ScoreDTO dto)
    {
        var newEntry = new RankingEntry(dto);

        // 기존 동일 플레이어 있으면 제거
        _rankingList.RemoveAll(e => e.PlayerId == dto.PlayerId);
        _rankingList.Add(newEntry);

        // 정렬 기준 적용
        _rankingList = _rankingList
            .OrderByDescending(e => e.Score)
            .ThenByDescending(e => e.IsCleared)
            .ThenBy(e => e.ElapsedPlayTime)
            .Take(MaxRankCount)
            .ToList();

        GameManager.Instance.SaveManager.SaveRankingData();

        OnRankingSortComplete(dto);
    }

    public List<RankingEntry> GetTopRankings()
    {
        return _rankingList;
    }

    public int? GetMyRanking(string myPlayerId)
    {
        for (int i = 0; i < _rankingList.Count; i++)
        {
            if (_rankingList[i].PlayerId == myPlayerId)
            {
                return i + 1;
            }
        }
        return null; // 랭킹에 없음
    }
}
