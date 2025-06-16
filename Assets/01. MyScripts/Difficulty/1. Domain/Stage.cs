using System;
using UnityEngine;

public class Stage
{
    private StageConfigSO _config;

    public int CurrentLoopCount { get; private set; }
    public bool IsReadyToAdvance { get; private set; }
    public bool HasEnteredBranchingSelection { get; private set; } // 5n 스테이지에서 분기 선택지에 진입했는지 여부

    // 다음 스테이지 정보 (플레이어 선택 또는 자동 결정 후 설정됨)
    public StageConfigSO NextDeterminedStageConfig { get; private set; }

    public int StageNumber => _config.StageNumber;
    public string StageName => _config.StageName;
    public string SceneName => _config.SceneName;
    public GameObject StageBossPrefab => _config.StageBossPrefab;
    public NextStageRoutingType NextStageRoutingType => _config.NextStageType;


    public Stage(StageConfigSO initialConfig, int initialLoopCount = 1)
    {
        if (initialConfig == null)
        {
            Debug.LogError("Stage must be initialized with a valid StageConfigSO.");
            return;
        }

        _config = initialConfig;
        CurrentLoopCount = initialLoopCount;
        IsReadyToAdvance = false;
        HasEnteredBranchingSelection = false;
        NextDeterminedStageConfig = null;
    }


    // 스테이지 완료 상태로 설정 (보스 처치 등 모든 조건 만족 시 StageManager에서 호출)
    // 이 시점에 다음 스테이지로 나아갈 포탈이 활성화됩니다.
    public void MarkReadyToAdvance()
    {
        if (IsReadyToAdvance) return;
        IsReadyToAdvance = true;
        Debug.Log($"Stage {StageName} (Loop: {CurrentLoopCount}) 가 준비완료되었습니다.");

        // 5n 스테이지일 경우 분기 선택지로 진입
        if (IsBranchingStage())
        {
            EnterBranchingSelection();
        }
        else if (_config.NextStageType == NextStageRoutingType.Specific)
        {
            // 5n이 아닌 스테이지는 자동으로 다음 스테이지 결정
            NextDeterminedStageConfig = _config.NextSpecificStage;
            Debug.Log($"자동으로 다음 스테이지 설정: {NextDeterminedStageConfig?.StageName}");
        }
        // GameEnd 타입은 다음 config가 null로 남을 것.
    }

    // 5n 스테이지에서 분기 선택지에 진입
    private void EnterBranchingSelection()
    {
        if (_config.NextStageType == NextStageRoutingType.Branching)
        {
            HasEnteredBranchingSelection = true;
            Debug.Log($"선택 분기 Stage {StageName}. 플레이어 선택 기다리는중");
            // 이 시점에 UI에 선택지를 띄우도록 StageManager에 알림?
        }
    }

    // 플레이어의 선택에 따라 다음 스테이지를 결정하는 행위
    // 이 메서드는 플레이어의 UI 선택 이벤트에 의해 StageManager에서 호출됩니다.
    public void SetNextRouteByPlayerChoice(bool chooseBossPath)
    {
        if (!IsReadyToAdvance)
        {
            throw new InvalidOperationException("너는 아직 준비가 안됐다!!!");
        }
        if (!HasEnteredBranchingSelection)
        {
            throw new InvalidOperationException("너는 분기 선택지에 들어가지 않았다!!!");
        }
        if (_config.NextStageType != NextStageRoutingType.Branching)
        {
            throw new InvalidOperationException("이 스테이지는 분기 선택이 불가능하다!!!");
        }

        if (chooseBossPath)
        {
            NextDeterminedStageConfig = _config.BossMapStage;
            Debug.Log($"플레이어가 보스를 선택: {_config.BossMapStage?.StageName}");
        }
        else
        {
            NextDeterminedStageConfig = _config.LoopTargetStage;
            Debug.Log($"플레이어가 루프를 선택: {_config.LoopTargetStage?.StageName}");
        }

        HasEnteredBranchingSelection = false; // 선택 완료
    }

    // 현재 스테이지가 5n 스테이지인지 확인
    public bool IsBranchingStage()
    {
        // 5, 10, 15, 20... 스테이지이면서 NextStageRoutingType이 Branching인 경우
        return _config.StageNumber > 0 && _config.StageNumber % 5 == 0 && _config.NextStageType == NextStageRoutingType.Branching;
    }


    // 다음 스테이지로 이동할 때 호출되어 새로운 Stage 인스턴스를 반환
    public Stage GetNextStageInstance()
    {
        if (NextDeterminedStageConfig == null) return null; // 게임 종료 또는 오류

        int nextLoopCount = CurrentLoopCount;
        // 루프 시작 지점(예: 1스테이지)으로 돌아갈 때 루프 카운트 증가
        // StageConfig.StageNumber는 고유 번호이고, 실제 루프가 도는지를 판단하는 기준
        if (NextDeterminedStageConfig.StageNumber < _config.StageNumber) // 5 -> 1 로 갈 때
        {
            nextLoopCount++;
            Debug.Log($"{NextDeterminedStageConfig.StageNumber}로 루프. 루프 카운트 {nextLoopCount}로 증가.");
        }

        // 새로운 Stage 인스턴스 생성
        return new Stage(NextDeterminedStageConfig, nextLoopCount);
    }

    // 현재 스테이지 정보 DTO 생성 (외부 시스템으로 데이터 전달용)
    public StageDTO ToDTO()
    {
        return new StageDTO(this);
    }
}