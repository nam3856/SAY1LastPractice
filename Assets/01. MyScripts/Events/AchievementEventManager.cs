using System;

public class AchievementEventManager
{
    public event Action<AchievementDTO> OnAchievementUpdated;
    public event Action<AchievementDTO> OnAchievementClaimed;
    public event Action<AchievementDTO> OnAchievementUnlocked;
    public void RaiseAchievementUpdated(AchievementDTO achievement)
    {
        OnAchievementUpdated?.Invoke(achievement);
    }
    public void RaiseAchievementClaimed(AchievementDTO achievement)
    {
        OnAchievementClaimed?.Invoke(achievement);
    }

    public void RaiseAchievementUnlocked(AchievementDTO achievement)
    {
        OnAchievementUnlocked?.Invoke(achievement);
    }
}