using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyConfigSO", menuName = "Game/Difficulty/Difficulty Config")]
public class DifficultyConfigSO : ScriptableObject
{
    [Header("시작 시 난이도 계수 초기값")]
    public float InitialDifficultyCoefficient = 1.0f;
    [Header("시간 경과에 따른 난이도 계수 증가 곡선")]
    public AnimationCurve DifficultyIncreaseCurve = new AnimationCurve(
    new Keyframe(0f, 1f),
    new Keyframe(300f, 5f),
    new Keyframe(600f, 25f),
    new Keyframe(2400f, 99f)
);
    [Header("스테이지 클리어 시 난이도 계수 증가 배율")]
    public float StageClearDifficultyMultiplier = 1.15f;
    [Header("적 체력 스케일링 기본 배율")]
    public float BaseEnemyHealthScale = 0.1f;
    [Header("적 공격력 스케일링 기본 배율")]
    public float BaseEnemyDamageScale = 0.05f;
    [Header("엘리트 적 스폰 확률 기본 배율")]
    public float BaseEliteSpawnChanceScale = 0.01f;

    [Header("최대 난이도 계수")]
    public float MaxDifficultyCoefficient = 100.0f;
}