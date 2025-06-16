using UnityEngine;

public class GameEventSystem
{
    public CurrencyEventManager Currency { get; } = new CurrencyEventManager();
    public AchievementEventManager Achievement { get; } = new AchievementEventManager();
    public AttendanceEventManager Attendance { get; } = new AttendanceEventManager();
    public DifficultyEventManager Difficulty { get; } = new DifficultyEventManager();
}