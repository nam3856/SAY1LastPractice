using UnityEngine;

public class AttendanceRewardDTO
{
    public readonly string DayName;
    public readonly ECurrencyType RewardCurrencyType;
    public readonly int RewardCurrencyAmount;
    public readonly bool IsClaimed;
    public readonly bool IsHighlight;

    // 핵심 도메인 + UI 정보 (IsHighlight)
    public AttendanceRewardDTO(AttendanceReward reward, bool isHighlight)
    {
        DayName = reward.DayName;
        RewardCurrencyType = reward.RewardCurrencyType;
        RewardCurrencyAmount = reward.RewardCurrencyAmount;
        IsClaimed = reward.IsClaimed;
        IsHighlight = isHighlight;
    }

    // 저장/불러오기용 순수 정보 DTO
    public AttendanceRewardDTO(string dayName, ECurrencyType rewardCurrencyType, int rewardCurrencyAmount, bool isClaimed)
    {
        DayName = dayName;
        RewardCurrencyType = rewardCurrencyType;
        RewardCurrencyAmount = rewardCurrencyAmount;
        IsClaimed = isClaimed;
        IsHighlight = false;
    }
}
