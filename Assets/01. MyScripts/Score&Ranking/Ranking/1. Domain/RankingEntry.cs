using UnityEngine;

public class RankingEntry
{
    public string PlayerId;
    public string Nickname;
    public int Score;
    public bool IsCleared;
    public float ElapsedPlayTime;

    public RankingEntry(ScoreDTO dto)
    {
        PlayerId = dto.PlayerId;
        Nickname = dto.Nickname;
        Score = dto.Highscore;
        IsCleared = dto.IsCleared;
        ElapsedPlayTime = dto.ElapsedPlayTime;
    }
}
