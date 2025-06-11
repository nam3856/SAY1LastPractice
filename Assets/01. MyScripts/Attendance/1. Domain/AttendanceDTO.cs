using UnityEngine;
using System;

public class AttendanceDTO
{
    public readonly int TotalAttendanceDays;
    public readonly DateTime LastAttendanceDate;

    public AttendanceDTO(Attendance attendance)
    {
        TotalAttendanceDays = attendance.TotalAttendanceDays;
        LastAttendanceDate = attendance.LastAttendanceDate;
    }
    public AttendanceDTO(int totalAttendanceDays, DateTime lastAttendanceDate)
    {
        TotalAttendanceDays = totalAttendanceDays;
        LastAttendanceDate = lastAttendanceDate;
    }
    public Attendance ToDomain()
    {
        return new Attendance(TotalAttendanceDays, LastAttendanceDate);
    }
    public AttendanceDTO ToDTO(Attendance attendance)
    {
        return new AttendanceDTO(attendance);
    }
}
