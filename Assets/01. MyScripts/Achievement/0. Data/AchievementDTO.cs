using System;

[System.Serializable]
public class AchievementDTO
{
    public string ID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public EAchievementCondition Condition { get; private set; }
    public int GoalValue { get; private set; }
    public int CurrentValue { get; private set; }
    public bool Rewarded { get; private set; }
    public ECurrencyType RewardCurrencyType { get; private set; }
    public int RewardCurrencyAmount { get; private set; }
    public DateTime ClaimedDate { get; private set; } = DateTime.MinValue;


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
    }

}