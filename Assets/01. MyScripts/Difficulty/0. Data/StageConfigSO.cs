using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageConfig_", menuName = "Game/Domain/StageConfig")]
public class StageConfigSO : ScriptableObject
{
    [Header("스테이지 고유 번호")]
    public int StageNumber;

    [Tooltip("스테이지의 이름 (UI 표시용)")]
    public string StageName;

    [Header("이 스테이지에서 로드될 씬의 이름")]
    public string SceneName;

    [Header("이 스테이지의 보스 몬스터 프리팹 (해당 스테이지 클리어 조건)")]
    public GameObject StageBossPrefab;

    // --- 다음 스테이지 연결 로직 ---
    [Header("다음 스테이지 타입")]
    public NextStageRoutingType NextStageType;

    [Tooltip("NextStageType이 Specific일 경우 연결될 다음 StageConfigSO")]
    public StageConfigSO NextSpecificStage; // 예: 1 -> 2, 2 -> 3, 3 -> 4, 4 -> 5 에 사용

    [Tooltip("NextStageType이 Loop일 경우, 루프 시작 지점으로 돌아갈 StageConfigSO (예: 5에서 루프 시 1로)")]
    public StageConfigSO LoopTargetStage;

    [Tooltip("NextStageType이 Boss일 경우 연결될 보스 맵의 StageConfigSO")]
    public StageConfigSO BossMapStage;
}

public enum NextStageRoutingType
{
    Specific, // 정해진 다음 스테이지로 이동 (예: 1->2, 2->3, 3->4)
    Branching, // 플레이어 선택에 따라 분기 (예: 5스테이지 -> 보스 OR 루프)
    GameEnd // 게임의 최종 엔딩 스테이지 (보스맵 클리어 후)
}