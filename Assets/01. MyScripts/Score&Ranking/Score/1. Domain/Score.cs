using System;

public class Score
{
    public string PlayerId { get; private set; }
    public int Currentscore { get; private set; }
    
    public int Highscore { get; private set; }

    public bool IsCleared { get; private set; }

    public float ElapsedPlayTime { get; private set; }

    private ScoreDTO _previousScore;

    public event Action OnHighScoreUpdated;


    public Score(string playerId,int currentScore, int highScore, bool isCleared, float elapsedPlayTime)
    {
        if (string.IsNullOrEmpty(playerId))
        {
            throw new System.ArgumentException("PlayerId cannot be null or empty.", nameof(playerId));
        }
        if (currentScore < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(currentScore), "Current score cannot be negative.");
        }
        if (highScore < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(highScore), "High score cannot be negative.");
        }
        if (elapsedPlayTime < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(elapsedPlayTime), "Elapsed play time cannot be negative.");
        }
        PlayerId = playerId;
        Currentscore = currentScore;
        Highscore = highScore;
        IsCleared = isCleared;
        ElapsedPlayTime = elapsedPlayTime;
    }
    public Score(ScoreDTO scoreDTO)
    {
        if (scoreDTO == null)
        {
            throw new System.ArgumentNullException(nameof(scoreDTO), "ScoreDTO cannot be null.");
        }
        PlayerId = scoreDTO.PlayerId;
        Currentscore = scoreDTO.Currentscore;
        Highscore = scoreDTO.Highscore;
        IsCleared = scoreDTO.IsCleared;
        ElapsedPlayTime = scoreDTO.ElapsedPlayTime;
        _previousScore = scoreDTO;
    }
    public void AddScore(int score)
    {
        if(score <= 0)
        {
            throw new System.ArgumentException("Score must be greater than zero.");
        }
        Currentscore += score;
    }

    public bool IsNewHighScore()
    {

        // 현재 스코어가 0이거나 이전 스코어보다 낮을 경우
        if (Currentscore == 0 || Highscore > Currentscore)
        {
            return false;
        }
        if (Currentscore > Highscore)
        {
            return true;
        }
        else if (Currentscore == Highscore)
        {
            // 현재 스코어가 최고 점수와 같을 경우 클리어 여부로 결정
            if (IsCleared && !_previousScore.IsCleared)
            {
                return true; // 클리어된 경우가 이전에 클리어되지 않은 경우보다 우선
            }
            else if (!IsCleared && _previousScore.IsCleared)
            {
                // 이전에 클리어된 경우가 현재 클리어되지 않은 경우보다 우선
                return false;
            }
            else
            {
                // 클리어 여부가 같을 경우, 플레이 시간으로 결정
                // 플레이 시간이 같을리는 없지만 만약에 같다면 이전 스코어가 더 빠른 것으로 간주
                return ElapsedPlayTime < _previousScore.ElapsedPlayTime;
            }
        }

        // 여기까지 올 경우는 없지만..
        return false;
    }

    public void UpdateHighScore()
    {
        if (IsNewHighScore())
        {
            Highscore = Currentscore;
            OnHighScoreUpdated?.Invoke();
        }
    }

    public void SetCleared(bool isCleared)
    {
        IsCleared = isCleared;
    }

    public void SetElapsedPlayTime(float elapsedPlayTime)
    {
        if (elapsedPlayTime <= 0)
        {
            throw new System.ArgumentException("Elapsed play time must be greater than zero.");
        }
        ElapsedPlayTime = elapsedPlayTime;
    }

    public ScoreDTO ToDTO()
    {
        return new ScoreDTO(this);
    }
}