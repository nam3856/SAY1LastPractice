using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyConfigSO _config;
    [SerializeField] private List<DifficultyTierSO> _difficultyTiers;

    private Difficulty _difficulty;

    private float _lastSliderCoefficient = -1f;
    private const float SliderStep = 0.02f;

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

        float current = _difficulty.CurrentCoefficient;
        if (Mathf.Abs(current - _lastSliderCoefficient) >= SliderStep)
        {
            _lastSliderCoefficient = current;
            GameManager.Instance.Events.Difficulty.RaiseSliderChanged(current);
        }

        //===================
        //Debug
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _isStageClearRequested = true;
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





    //============================================
    // Debug
    private bool _isStageClearRequested;
    private void OnGUI()
    {
        if (_difficulty == null) return;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            normal = { textColor = Color.white }
        };

        float elapsed = _difficulty.ToDTO().CurrentElapsedTime;
        float coefficient = _difficulty.ToDTO().CurrentCoefficient;
        string tierName = _difficulty.ToDTO().CurrentTierName;

        string message = $"Elapsed Time: {elapsed:F1}s\n" +
                         $"Coefficient: {coefficient:F2}\n" +
                         $"Tier: {tierName}";

        GUI.Label(new Rect(20, 500, 400, 100), message, labelStyle);
        GUI.Button(new Rect(20, 610, 200, 40), "Clear Stage (F5)");
        if (_isStageClearRequested)
        {
            _isStageClearRequested = false;
            NotifyStageCleared();
            Debug.Log("[DEBUG] Stage Cleared! Coefficient boosted.");
        }
    }

}
