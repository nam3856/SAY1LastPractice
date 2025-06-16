using UnityEngine;

public class AttendanceReward
{
    public string DayName { get; }
    public ECurrencyType RewardCurrencyType { get; }
    public int RewardCurrencyAmount { get; }
    public bool IsClaimed { get; private set; }

    public AttendanceReward(string dayName, ECurrencyType rewardCurrencyType, int rewardCurrencyAmount)
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
    public bool MarkAsClaimedIfPossible()
    {
        if(CanClaimReward() == false)
        {
            return false;
        }

        IsClaimed = true;
        return true;
    }

    public void ResetClaim()
    {
        IsClaimed = false;
    }

}
