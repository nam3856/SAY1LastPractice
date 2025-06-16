using Unity.FPS.Game;
using UnityEngine;

public class EnemyStatProvider : MonoBehaviour
{

    private Health _health;
    private float _initialMaxHealth;

    public void Awake()
    {
        _health = GetComponent<Health>();
        _initialMaxHealth = _health.MaxHealth;
    }

    public void Start()
    {
        ApplyStatsFromDifficulty();
    }

    public void ApplyStatsFromDifficulty()
    {
        // 체력 비율 유지
        float currentRatio = _health.CurrentHealth / _health.MaxHealth;

        // DifficultyManager에서 새로운 체력/공격력 받아오기
        var difficultyManager = GameManager.Instance.GetComponent<DifficultyManager>();
        if (difficultyManager == null)
        {
            Debug.LogError("[EnemyStatProvider] DifficultyManager를 찾을 수 없습니다.");
            return;
        }

        // 난이도 반영된 최대 체력 계산
        float newMaxHealth = difficultyManager.GetEnemyHealth(_initialMaxHealth);
        _health.MaxHealth = newMaxHealth;

        // 현재 체력은 이전 비율 유지하면서 갱신
        _health.CurrentHealth = newMaxHealth * currentRatio;

        // 공격력 갱신


        //Debug.Log($"[EnemyStatProvider] 체력 {newMaxHealth}, 공격력 {newDamage} 적용 완료");
    }
}
