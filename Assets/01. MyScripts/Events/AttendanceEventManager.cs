using System;

public class AttendanceEventManager
{
    public event Action OnAttendanceInitialized;
    public event Action<bool> OnTodayAttendanceChecked;
    public event Action<AttendanceRewardDTO> OnAttendanceRewardClaimed;
    public void RaiseAttendanceInitialized()
    {
        OnAttendanceInitialized?.Invoke();
    }
    public void RaiseTodayAttendanceChecked(bool check)
    {
        OnTodayAttendanceChecked?.Invoke(check);
    }
    public void RaiseRewardClaimed(AttendanceRewardDTO attendanceSlotDTO)
    {

        OnAttendanceRewardClaimed?.Invoke(attendanceSlotDTO);
    }

}