using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyConfigSO _config;
    [SerializeField] private List<DifficultyTierSO> _difficultyTiers;

    private Difficulty _difficulty;

    private float _lastSliderCoefficient = -1f;
    private const float SliderStep = 0.1f;

    private void Awake()
    {
        if (_config == null || _difficultyTiers == null || _difficultyTiers.Count == 0)
        {
            Debug.LogError("DifficultyManager 초기화 실패: 설정이 비어 있음.");
            enabled = false;
            return;
        }

        _difficulty = new Difficulty(_config, _difficultyTiers);
        _difficulty.DifficultyTierChanged += HandleDifficultyTierChanged;
    }

    private void Update()
    {
        _difficulty.UpdateTime(Time.deltaTime);

        float current = Mathf.Floor(_difficulty.CurrentCoefficient * 10f) * SliderStep;
        if (!Mathf.Approximately(current, _lastSliderCoefficient))
        {
            _lastSliderCoefficient = current;
            GameManager.Instance.Events.Difficulty.RaiseSliderChanged(current);
        }
    }

    public void NotifyStageCleared()
    {
        _difficulty.StageCleared();
    }

    private void HandleDifficultyTierChanged(DifficultyTierSO newTier)
    {
        GameManager.Instance.Events.Difficulty.RaiseTierChanged(_difficulty.ToDTO());
    }

    private void OnDestroy()
    {
        if (_difficulty != null)
            _difficulty.DifficultyTierChanged -= HandleDifficultyTierChanged;
    }

    // 외부 시스템용 API
    public float GetEnemyHealth(float baseHealth)
        => _difficulty.GetScaledEnemyHealth(baseHealth);

    public float GetEnemyDamage(float baseDamage)
        => _difficulty.GetScaledEnemyDamage(baseDamage);

    public float GetEliteSpawnChance(float baseChance)
        => _difficulty.GetEliteSpawnChance(baseChance);

    public int GetBossAsNormalEnemyCount()
        => _difficulty.GetBossAsNormalEnemyCount();

    public DifficultyDTO ToDTO()
        => _difficulty.ToDTO();
}
