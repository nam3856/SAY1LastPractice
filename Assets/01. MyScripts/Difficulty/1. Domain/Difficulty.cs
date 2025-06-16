using System;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty
{
    private DifficultyConfigSO _config;
    private float _currentElapsedTime;
    private float _currentCoefficient;
    private IReadOnlyList<DifficultyTierSO> _allDifficultyTiers;
    public event Action<DifficultyTierSO> DifficultyTierChanged;
    private float _stageClearTimeOffset = 0f;

    public DifficultyTierSO CurrentActiveTier { get; private set; }

    public float CurrentCoefficient => _currentCoefficient;
    public Difficulty(DifficultyConfigSO config, IReadOnlyList<DifficultyTierSO> allDifficultyTiers)
    {
        _config = config;
        _allDifficultyTiers = allDifficultyTiers;
        _currentElapsedTime = 0f;
        _currentCoefficient = _config.InitialDifficultyCoefficient;
        UpdateCurrentTier();
    }

    // 시간 경과 업데이트
    public void UpdateTime(float deltaTime)
    {
        if(deltaTime < 0f)
        {
            throw new System.ArgumentOutOfRangeException(nameof(deltaTime), "Delta time은 0 미만이 될 수 없습니다.");
        }
        _currentElapsedTime += deltaTime;
        CalculateCurrentCoefficient();
    }

    // 스테이지 클리어 시 난이도 계수 증가
    public void StageCleared()
    {
        float currentCurveValue = _config.DifficultyIncreaseCurve.Evaluate(_currentElapsedTime + _stageClearTimeOffset);
        float target = _currentCoefficient * _config.StageClearDifficultyMultiplier;

        float newOffset = FindTimeOffsetToReach(target);

        // 계수 상승 이후 티어 갱신
        _stageClearTimeOffset = newOffset;
        CalculateCurrentCoefficient();
    }



    // 현재 난이도 티어 계산
    private void CalculateCurrentCoefficient()
    {
        float effectiveTime = _currentElapsedTime + _stageClearTimeOffset;
        _currentCoefficient = _config.DifficultyIncreaseCurve.Evaluate(effectiveTime);

        _currentCoefficient = Mathf.Min(_currentCoefficient, _config.MaxDifficultyCoefficient);
        UpdateCurrentTier();
    }

    private float FindTimeOffsetToReach(float targetValue)
    {
        float low = 0f;
        float high = 2400f;

        while (high - low > 0.1f)
        {
            float mid = (low + high) / 2f;
            float val = _config.DifficultyIncreaseCurve.Evaluate(_currentElapsedTime + mid);
            if (val < targetValue)
                low = mid;
            else
                high = mid;
        }

        return low;
    }

    private void UpdateCurrentTier()
    {
        DifficultyTierSO newTier = null;
        foreach (var tier in _allDifficultyTiers)
        {
            if (_currentCoefficient >= tier.MinDifficultyCoefficient)
            {
                newTier = tier;
            }
            else
            {
                break;
            }
        }

        if (newTier != null && newTier != CurrentActiveTier)
        {
            CurrentActiveTier = newTier;

            DifficultyTierChanged?.Invoke(CurrentActiveTier);

        }
    }

    public float GetScaledEnemyHealth(float baseHealth)
    {
        return baseHealth * (1 + (_currentCoefficient - 1) * _config.BaseEnemyHealthScale) * CurrentActiveTier.EnemyHealthMultiplier;
    }

    public float GetScaledEnemyDamage(float baseDamage)
    {
        return baseDamage * (1 + (_currentCoefficient - 1) * _config.BaseEnemyDamageScale) * CurrentActiveTier.EnemyDamageMultiplier;
    }

    public float GetEliteSpawnChance(float baseChance)
    {
        return baseChance + (_currentCoefficient - 1) * _config.BaseEliteSpawnChanceScale + CurrentActiveTier.EliteSpawnRate;
    }

    public int GetBossAsNormalEnemyCount()
    {
        return CurrentActiveTier.BossAsNormalEnemyCount;
    }

    public string GetDifficultyTierName()
    {
        return CurrentActiveTier?.Name ?? "Unknown";
    }

    public DifficultyDTO ToDTO()
    {
        return new DifficultyDTO
        (
            _currentElapsedTime,
            _currentCoefficient,
            GetDifficultyTierName()
        );
    }
}
