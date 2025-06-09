using Unity.FPS.Game;
using System;
using UnityEngine;
public class PlayerUpgradeService
{
    private readonly CurrencyManager _currencyManager;
    private readonly Health _health;

    public PlayerUpgradeService(CurrencyManager currencyManager, Health health)
    {
        _currencyManager = currencyManager;
        _health = health;
    }

    public bool TryUpgradeHealth(int cost, float increaseAmount)
    {
        var currency = _currencyManager.Currencies[ECurrencyType.Gold];
        if (!currency.TrySpend(cost))
        {
            Debug.Log("골드 부족");
            return false;
        }

        float ratio = _health.GetRatio();
        _health.MaxHealth += increaseAmount;
        _health.CurrentHealth = _health.MaxHealth * ratio;

        return true;
    }
}