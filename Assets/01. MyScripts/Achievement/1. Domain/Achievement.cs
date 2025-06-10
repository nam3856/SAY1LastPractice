using System;

public class Achievement
{
    public readonly string ID;
    public readonly string Name;
    public readonly string Description;
    public readonly EAchievementCondition Condition;
    public int GoalValue;
    public ECurrencyType RewardCurrencyType;
    public int RewardCurrencyAmount;
    private int _currentValue = 0;
    public int CurrentValue => _currentValue;
    private bool _rewarded = false;
    public bool Rewarded => _rewarded;
    public DateTime ClaimedDate { get; private set; } = DateTime.MinValue;

    public Achievement(AchievementDataSO metaData)
    {
        if (metaData == null)
        {
            throw new System.ArgumentNullException(nameof(metaData), "업적 데이터는 null일 수 없습니다.");
        }
        if (string.IsNullOrEmpty(metaData.ID))
            throw new System.ArgumentException("업적 ID는 비어있을 수 없습니다.", nameof(metaData.ID));
        if (string.IsNullOrEmpty(metaData.Name))
        {
            throw new System.ArgumentException("업적 이름은 비어있을 수 없습니다.", nameof(metaData.Name));
        }
        if (string.IsNullOrEmpty(metaData.Description))
        {
            throw new System.ArgumentException("업적 설명은 비어있을 수 없습니다.", nameof(metaData.Description));
        }
        if (metaData.GoalValue <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(metaData.GoalValue), "업적 목표 값은 0보다 커야 합니다.");
        }
        if (metaData.RewardCurrencyAmount < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(metaData.RewardCurrencyAmount), "보상 화폐 금액은 0 이상이어야 합니다.");
        }
        ID = metaData.ID;
        Name = metaData.Name;
        Description = metaData.Description;
        Condition = metaData.Condition;
        GoalValue = metaData.GoalValue;
        RewardCurrencyType = metaData.RewardCurrencyType;
        RewardCurrencyAmount = metaData.RewardCurrencyAmount;
    }


    // 세이브된 값 로드
    public Achievement(AchievementDataSO meta, AchievementDTO saved)
    : this(meta)
    {
        _currentValue = saved.CurrentValue;
        _rewarded = saved.Rewarded;
        ClaimedDate = saved.ClaimedDate;
    }


    public void Increase(int value)
    {
        if(value <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(value), "증가 값은 0보다 커야 합니다.");
        }

        _currentValue += value;
    }

    public bool CanClaimReward()
    {
        return _rewarded == false && _currentValue >= GoalValue;
    }

    public bool TryClaim()
    {
        if(CanClaimReward() == false)
        {
            return false;
        }

        _rewarded = true;
        ClaimedDate = DateTime.Now;
        return true;
    }

}