using System;

public class AttendanceEventManager
{
    public event Action OnAttendanceInitialized;
    public event Action OnTodayAttendanceChecked;
    public event Action<ECurrencyType, int> OnAttendanceRewardClaimed;
    public void RaiseAttendanceInitialized()
    {
        OnAttendanceInitialized?.Invoke();
    }
    public void RaiseTodayAttendanceChecked()
    {
        OnTodayAttendanceChecked?.Invoke();
    }
    public void RaiseRewardClaimed(AttendanceSlotDTO attendanceSlotDTO)
    {

        OnAttendanceRewardClaimed?.Invoke(attendanceSlotDTO.RewardCurrencyType, attendanceSlotDTO.RewardCurrencyAmount);
    }

}