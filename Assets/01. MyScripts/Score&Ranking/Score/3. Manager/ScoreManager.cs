using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public Score CurrentScore { get; private set; }
    public string PlayerId { get; private set; }

    public int CurrentScoreValue => CurrentScore?.Currentscore ?? 0;


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

    public void Initialize(string accountID, ScoreDTO scoreDTO = null)
    {
        if(scoreDTO == null)
        {
            // 점수 데이터가 없을 경우 기본값으로 초기화
            CurrentScore = new Score(accountID, 0, 0, false, 0f);
        }
        else
        {
            // 로드된 점수 데이터가 있다면 해당 데이터를 사용하여 Score 객체를 초기화
            CurrentScore = new Score(scoreDTO);
        }
        PlayerId = accountID;
        if (CurrentScore == null)
        {
            throw new System.InvalidOperationException("CurrentScore is not initialized.");
        }
        CurrentScore.OnHighScoreUpdated += HandleHighScoreUpdated;

        GameManager.Instance.Events.Session.OnSessionEnded += HandleSessionEnded;
    }
    private void OnDestroy()
    {
        CurrentScore.OnHighScoreUpdated -= HandleHighScoreUpdated;

        if(GameManager.Instance != null)
        {
            GameManager.Instance.Events.Session.OnSessionEnded -= HandleSessionEnded;
        }
    }

    private void HandleSessionEnded(PlaySessionDTO sessionDTO)
    {
        if(sessionDTO == null)
        {
            Debug.LogWarning("SessionDTO is null in HandleSessionEnded.");
            return;
        }
        SetCleared(sessionDTO.IsCleared);
        SetElapsedPlayTime(sessionDTO.ElapsedPlayTime);
        if (UpdateHighScore())
        {
            GameManager.Instance.Events.Score.RaiseHighScoreUpdated(CurrentScore.ToDTO());
        }
        else
        {
            GameManager.Instance.Events.Score.RaiseScoreCalculateFinished(CurrentScore.ToDTO());
        }
    }

    private void HandleHighScoreUpdated()
    {
        // 하이스코어가 업데이트되었을 때의 처리 로직
        GameManager.Instance.SaveManager.SaveScoreData();
        Debug.Log("High score updated: " + CurrentScore.Highscore);
    }

    public void AddScore(int score)
    {
        CurrentScore.AddScore(score);
        GameManager.Instance.Events.Score.RaiseScoreUpdated(CurrentScore.ToDTO());
    }

    public void SetCleared(bool isCleared)
    {
        CurrentScore.SetCleared(isCleared);
    }

    public void SetElapsedPlayTime(float elapsedPlayTime)
    {
        CurrentScore.SetElapsedPlayTime(elapsedPlayTime);
    }

    public bool IsNewHighScore()
    {
        return CurrentScore.IsNewHighScore();
    }

    public bool UpdateHighScore()
    {
        if(IsNewHighScore())
        {
            CurrentScore.UpdateHighScore();
            return true;
        }
        return false;
    }

    public ScoreDTO GetScoreDTO()
    {
        if (CurrentScore == null)
        {
            throw new System.InvalidOperationException("CurrentScore is not initialized.");
        }
        return CurrentScore.ToDTO();
    }

}
