public class PlaySessionDTO
{
    public readonly float ElapsedPlayTime;

    public readonly bool IsCleared;

    public PlaySessionDTO(PlaySession session)
    {
        ElapsedPlayTime = session.ElapsedPlayTime;
        IsCleared = session.IsCleared;
    }
}