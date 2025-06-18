using Firebase.Firestore;
using UnityEngine;
[FirestoreData]
public class RankingEntry
{
    [FirestoreProperty]
    public string PlayerId { get; set; }

    [FirestoreProperty]
    public string Nickname { get; set; }

    [FirestoreProperty]
    public int Score { get; set; }

    [FirestoreProperty]
    public bool IsCleared { get; set; }

    [FirestoreProperty]
    public float ElapsedPlayTime { get; set; }

    public RankingEntry() { }
    public RankingEntry(ScoreDTO dto)
    {
        PlayerId = dto.PlayerId;
        Nickname = dto.Nickname;
        Score = dto.Highscore;
        IsCleared = dto.IsCleared;
        ElapsedPlayTime = dto.ElapsedPlayTime;
    }

    public RankingEntry( int score, string playerId, string nickname, bool isCleared, float elapsedPlayTime)
    {
        PlayerId = playerId;
        Nickname = nickname;
        Score = score;
        IsCleared = isCleared;
        ElapsedPlayTime = elapsedPlayTime;
    }
}
