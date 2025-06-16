using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "Difficulty/Create New DifficultyData")]

public class DifficultyDataSO : ScriptableObject
{
    [Header("난이도 종류")]
    public EDifficulty DifficultyType;
    [Header("난이도 이름")]
    public string Name;
    [Header("난이도 레벨")]
    public int Level;
    [Header("적 체력 비율")]
    public float EnemyHealthMultiplier = 1.0f;
    [Header("적 공격력 비율")]
    public float EnemyDamageMultiplier = 1.0f;
    [Header("엘리트 적 스폰 빈도")]
    public float EliteSpawnRate = 0.1f;
    [Header("보스 적 스폰 빈도")]
    public float BossSpawnRate = 0.05f;
}
