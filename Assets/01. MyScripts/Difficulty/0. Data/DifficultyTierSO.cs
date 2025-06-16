using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyTier", menuName = "Difficulty/Create New DifficultyTier")]

public class DifficultyTierSO : ScriptableObject
{
    [Header("최소 난이도 계수")]
    public float MinDifficultyCoefficient;
    [Header("난이도 종류")]
    public EDifficulty DifficultyType;
    [Header("난이도 이름")]
    public string Name;
    [Header("적 체력 비율")]
    public float EnemyHealthMultiplier = 1.0f;
    [Header("적 공격력 비율")]
    public float EnemyDamageMultiplier = 1.0f;
    [Header("엘리트 적 스폰 빈도")]
    [Range(0f, 1f)]
    public float EliteSpawnRate = 0.1f;
    [Header("보스 적 일반스폰 갯수")]
    public int BossAsNormalEnemyCount = 0;
}
