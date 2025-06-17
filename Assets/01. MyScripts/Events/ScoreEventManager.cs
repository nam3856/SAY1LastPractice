using System;

public class ScoreEventManager
{
    public event Action<ScoreDTO> OnScoreUpdated;
    public event Action<ScoreDTO> OnHighScoreUpdated;
    public event Action<ScoreDTO> OnScoreCalculateFinished;

    public void RaiseScoreUpdated(ScoreDTO scoreDTO)
    {
        OnScoreUpdated?.Invoke(scoreDTO);
    }
    public void RaiseHighScoreUpdated(ScoreDTO scoreDTO)
    {
        OnHighScoreUpdated?.Invoke(scoreDTO);
    }
    public void RaiseScoreCalculateFinished(ScoreDTO scoreDTO)
    {
        OnScoreCalculateFinished?.Invoke(scoreDTO);
    }
}