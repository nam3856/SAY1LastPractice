using UnityEngine;

public class AttendanceSlotDTO
{
    public readonly string DayName;
    public readonly ECurrencyType RewardCurrencyType;
    public readonly int RewardCurrencyAmount;
    public readonly bool IsClaimed;
    public readonly bool IsHighlight;

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

    public AttendanceSlotDTO(AttendanceSlot slot, int currentAttendanceDay, int slotIndex)
    {
        DayName = slot.DayName;
        RewardCurrencyType = slot.RewardCurrencyType;
        RewardCurrencyAmount = slot.RewardCurrencyAmount;
        IsClaimed = slot.IsClaimed;

        IsHighlight = slotIndex < currentAttendanceDay && !slot.IsClaimed;
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
