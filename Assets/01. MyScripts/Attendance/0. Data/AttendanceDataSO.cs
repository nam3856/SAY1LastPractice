using UnityEngine;

[CreateAssetMenu(fileName = "AttendanceData", menuName = "Attendance/Create New AttendanceData")]

public class AttendanceDataSO : ScriptableObject
{
    [Header("몇 일 차?")]
    public int Day;
    [Header("보상 화폐 종류")]
    public ECurrencyType RewardCurrencyType;
    [Header("보상 화폐 금액")]
    public int RewardCurrencyAmount;

}