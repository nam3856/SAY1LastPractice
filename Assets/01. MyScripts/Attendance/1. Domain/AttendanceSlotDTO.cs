using UnityEngine;

public class AttendanceSlotDTO
{
    public readonly string DayName;
    public readonly ECurrencyType RewardCurrencyType;
    public readonly int RewardCurrencyAmount;
    public readonly bool IsClaimed;

    public AttendanceSlotDTO(AttendanceSlot attendanceSlot)
    {
        DayName = attendanceSlot.DayName;
        RewardCurrencyType = attendanceSlot.RewardCurrencyType;
        RewardCurrencyAmount = attendanceSlot.RewardCurrencyAmount;
        IsClaimed = attendanceSlot.IsClaimed;
    }
    public AttendanceSlotDTO(string dayName, ECurrencyType rewardCurrencyType, int rewardCurrencyAmount, bool isClaimed)
    {
        DayName = dayName;
        RewardCurrencyType = rewardCurrencyType;
        RewardCurrencyAmount = rewardCurrencyAmount;
        IsClaimed = isClaimed;
    }
    public AttendanceSlot ToDomain()
    {
        return new AttendanceSlot(DayName, RewardCurrencyType, RewardCurrencyAmount);
    }
    public AttendanceSlotDTO ToDTO(AttendanceSlot attendanceSlot)
    {
        return new AttendanceSlotDTO(attendanceSlot);
    }
}
