using System;

public class AttendanceEventManager
{
    public event Action OnAttendanceInitialized;
    public event Action OnTodayAttendanceChecked;
    public event Action<AttendanceSlotDTO> OnAttendanceRewardClaimed;
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

        OnAttendanceRewardClaimed?.Invoke(attendanceSlotDTO);
    }

}