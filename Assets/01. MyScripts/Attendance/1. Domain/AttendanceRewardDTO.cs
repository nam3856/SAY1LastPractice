using UnityEngine;

public class AttendanceRewardDTO
{
    public readonly string DayName;
    public readonly ECurrencyType RewardCurrencyType;
    public readonly int RewardCurrencyAmount;
    public readonly bool IsClaimed;
    public readonly bool IsHighlight;

    // �ٽ� ������ + UI ���� (IsHighlight)
    public AttendanceRewardDTO(AttendanceReward reward, bool isHighlight)
    {
        DayName = reward.DayName;
        RewardCurrencyType = reward.RewardCurrencyType;
        RewardCurrencyAmount = reward.RewardCurrencyAmount;
        IsClaimed = reward.IsClaimed;
        IsHighlight = isHighlight;
    }

    // ����/�ҷ������ ���� ���� DTO
    public AttendanceRewardDTO(string dayName, ECurrencyType rewardCurrencyType, int rewardCurrencyAmount, bool isClaimed)
    {
        DayName = dayName;
        RewardCurrencyType = rewardCurrencyType;
        RewardCurrencyAmount = rewardCurrencyAmount;
        IsClaimed = isClaimed;
        IsHighlight = false;
    }
}
