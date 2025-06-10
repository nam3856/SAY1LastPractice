using System;
using static AchievementRepository;

[System.Serializable]
public class AchievementDTO
{
    public readonly string ID;
    public readonly string Name;
    public readonly string Description;
    public readonly EAchievementCondition Condition;
    public readonly int GoalValue;
    public readonly int CurrentValue;
    public readonly bool Rewarded;
    public readonly ECurrencyType RewardCurrencyType;
    public readonly int RewardCurrencyAmount;
    public readonly DateTime ClaimedDate;
    public readonly bool IsUnlocked;


    public AchievementDTO(Achievement achievement)
    {
        if (string.IsNullOrEmpty(achievement.ID))
            throw new ArgumentException("Achievement ID는 비어있을 수 없습니다.");
        ID = achievement.ID;
        Name = achievement.Name;
        Description = achievement.Description;
        Condition = achievement.Condition;
        GoalValue = achievement.GoalValue;
        CurrentValue = achievement.CurrentValue;
        Rewarded = achievement.Rewarded;
        RewardCurrencyType = achievement.RewardCurrencyType;
        RewardCurrencyAmount = achievement.RewardCurrencyAmount;
        ClaimedDate = achievement.ClaimedDate;
        IsUnlocked = achievement.IsUnlocked;
    }

    public AchievementDTO(AchievementSaveModel achievement)
    {
        if (string.IsNullOrEmpty(achievement.ID))
            throw new ArgumentException("Achievement ID는 비어있을 수 없습니다.");
        ID = achievement.ID;
        Name = achievement.Name;
        Description = achievement.Description;
        Condition = achievement.Condition;
        GoalValue = achievement.GoalValue;
        CurrentValue = achievement.CurrentValue;
        Rewarded = achievement.Rewarded;
        RewardCurrencyType = achievement.RewardCurrencyType;
        RewardCurrencyAmount = achievement.RewardCurrencyAmount;
        ClaimedDate = string.IsNullOrEmpty(achievement.ClaimedDate) ? DateTime.MinValue : DateTime.Parse(achievement.ClaimedDate);
        IsUnlocked = achievement.IsUnlocked;
    }

    public AchievementSaveModel ToSaveModel()
    {
        return new AchievementSaveModel
        {
            ID = ID,
            Name = Name,
            Description = Description,
            Condition = Condition,
            GoalValue = GoalValue,
            CurrentValue = CurrentValue,
            Rewarded = Rewarded,
            RewardCurrencyType = RewardCurrencyType,
            RewardCurrencyAmount = RewardCurrencyAmount,
            ClaimedDate = ClaimedDate.ToString("o"),
            IsUnlocked = IsUnlocked
        };
    }
}