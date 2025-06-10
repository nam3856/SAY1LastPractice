using System;

public class AchievementEventManager
{
    public event Action<AchievementDTO> OnAchievementUpdated;
    public event Action<AchievementDTO> OnAchievementClaimed;

    public void RaiseAchievementUpdated(AchievementDTO achievement)
    {
        OnAchievementUpdated?.Invoke(achievement);
    }
    public void RaiseAchievementClaimed(AchievementDTO achievement)
    {
        OnAchievementClaimed?.Invoke(achievement);
    }
}