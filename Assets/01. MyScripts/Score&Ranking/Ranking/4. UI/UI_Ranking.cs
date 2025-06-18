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
    public static UI_Ranking Instance { get; private set; }
    private System.Action _onClosed;
    [SerializeField] private CanvasGroup canvasGroup;
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
        GameManager.Instance.Events.Ranking.OnRankingUpdated += OnRankingUpdated;

        InitEmptyRanking();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Events.Ranking.OnRankingUpdated -= OnRankingUpdated;
        }
    }

    public async void OnRankingUpdated()
    {
        var topRankings = await RankingManager.Instance.GetTopRankings();

        int index = 1;
        foreach (var ranking in topRankings)
        {
            if (index - 1 >= entryUIList.Count)
                break;

            entryUIList[index - 1].SetData(index.ToString(), ranking.Nickname, ranking.Score.ToString("N0"));
            index++;
        }

        string myId = AccountManager.Instance?.GetMyEmail() ?? "";
        var myRankIndex = topRankings.FindIndex(e => e.PlayerId == myId);
        if (myRankIndex != -1)
        {
            Debug.Log($"내 순위: {myRankIndex + 1}, 이름: {AccountManager.Instance?.GetMyNickname() ?? ""}, 최고점수: {ScoreManager.Instance.CurrentScore.Highscore}");
        }
        else
        {
            Debug.Log("순위 안에 들지 못했습니다.");
        }
    }

    public void Show(System.Action onClosedCallback)
    {
        _onClosed = onClosedCallback;
        canvasGroup.alpha = 1f; // UI 활성화
        canvasGroup.interactable = true; // UI 상호작용 가능
        canvasGroup.blocksRaycasts = true; // UI 블록 레이캐스트 가능
    }

    public void Close()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false; // UI 상호작용 불가능
        canvasGroup.blocksRaycasts = false; // UI 블록 레이캐스트 불가능
        _onClosed?.Invoke();         // 콜백 실행
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
