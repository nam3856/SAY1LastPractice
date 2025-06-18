using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }
    private List<RankingEntry> _rankingList;
    private const string COLLECTION_NAME = "rankings";
    public int MaxRankCount = 10;

    private FirestoreRankingUploader _firestoreRankingUploader;
    private FirebaseFirestore _db;


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
    private void Start()
    {

        Initialize();
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
        _db = FirebaseFirestore.DefaultInstance;
        _firestoreRankingUploader = new();
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
    public async void TryAddRanking(ScoreDTO dto)
    {
        await _firestoreRankingUploader.UploadIfHighScoreAsync(dto);

        OnRankingSortComplete(dto);
    }

    public async Task<List<RankingEntry>> GetTopRankings()
    {
        try
        {
            Debug.Log("Firestore 랭킹 쿼리 시작");

            var snapshot = await _db.Collection(COLLECTION_NAME)
                .OrderByDescending("Score")
                .OrderByDescending("IsCleared")
                .OrderBy("ElapsedPlayTime")
                .Limit(MaxRankCount)
                .GetSnapshotAsync();

            Debug.Log($"Firestore 쿼리 완료. 총 문서 수: {snapshot.Count}");

            var result = snapshot.Documents.Select(doc =>
            {
                Debug.Log($"문서 ID: {doc.Id}, 데이터: {doc.ToDictionary().ToString()}");
                return doc.ConvertTo<RankingEntry>();
            }).ToList();

            return result;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Firestore 랭킹 쿼리 실패: {e.Message}");
            return new List<RankingEntry>();
        }
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
