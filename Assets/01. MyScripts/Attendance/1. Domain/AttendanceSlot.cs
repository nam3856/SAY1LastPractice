using UnityEngine;

public class AttendanceSlot
{
    public string DayName;
    public ECurrencyType RewardCurrencyType;
    public int RewardCurrencyAmount;
    public bool IsClaimed;

    public AttendanceSlot(string dayName, ECurrencyType rewardCurrencyType, int rewardCurrencyAmount)
    {
        if(string.IsNullOrEmpty(dayName))
        {
            throw new System.ArgumentNullException(nameof(dayName), "DayName은 비어있을 수 없습니다.");
        }
        if (rewardCurrencyAmount < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(rewardCurrencyAmount), "보상 화폐 금액은 0 이상이어야 합니다.");
        }

        DayName = dayName;
        RewardCurrencyType = rewardCurrencyType;
        RewardCurrencyAmount = rewardCurrencyAmount;
    }
    public bool CanClaimReward()
    {
        return IsClaimed == false;
    }
    public bool TryClaim()
    {
        if(CanClaimReward() == false)
        {
            return false;
        }

        IsClaimed = true;
        return true;
    }
}
