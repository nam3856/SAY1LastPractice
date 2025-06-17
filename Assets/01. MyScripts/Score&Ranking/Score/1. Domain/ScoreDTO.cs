public class ScoreDTO
{
    public readonly int Currentscore;
    public readonly int Highscore;
    public readonly bool IsCleared;
    public readonly float ElapsedPlayTime;
    public readonly string PlayerId;
    public readonly string Nickname;

    public ScoreDTO(Score score)
    {
        if (score == null)
        {
            throw new System.ArgumentNullException(nameof(score), "Score cannot be null.");
        }
        PlayerId = score.PlayerId;
        Currentscore = score.Currentscore;
        Highscore = score.Highscore;
        IsCleared = score.IsCleared;
        ElapsedPlayTime = score.ElapsedPlayTime;
    }

    public ScoreDTO(ScoreSaveModel scoreSaveModel)
    {
        PlayerId = scoreSaveModel.PlayerId;
        Currentscore = 0;
        Highscore = scoreSaveModel.Score;
        IsCleared = scoreSaveModel.IsCleared;
        ElapsedPlayTime = scoreSaveModel.ElapsedTime;
        Nickname = AccountManager.Instance.GetNicknameByPlayerId(PlayerId);
    }

    public Score ToDomain()
    {
        return new Score(this);
    }

}