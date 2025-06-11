using System;
using UnityEngine;

public class Attendance
{
    public int TotalAttendanceDays;
    public DateTime LastAttendanceDate;

    public Attendance(int totalAttendanceDays, DateTime lastAttendanceDate)
    {
        if(totalAttendanceDays < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(totalAttendanceDays), "총 출석 일수는 0 이상이어야 합니다.");
        }
        if(lastAttendanceDate < DateTime.MinValue)
        {
            throw new System.ArgumentOutOfRangeException(nameof(lastAttendanceDate), "마지막 출석 날짜는 최소값 이상이어야 합니다.");
        }

        TotalAttendanceDays = totalAttendanceDays;
        LastAttendanceDate = lastAttendanceDate;
    }

    public void Increase(int value)
    {
        if(value <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(value), "증가 값은 0보다 커야 합니다.");
        }

        TotalAttendanceDays += value;
        LastAttendanceDate = DateTime.Now;
    }

    public bool CheckTodayAttendance()
    {
        return LastAttendanceDate.Date < DateTime.Now.Date;
    }
}
