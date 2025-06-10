using UnityEngine;

public class GameEventSystem
{
    public CurrencyEventManager Currency { get; } = new CurrencyEventManager();
    public AchievementEventManager Achievement { get; } = new AchievementEventManager();
}