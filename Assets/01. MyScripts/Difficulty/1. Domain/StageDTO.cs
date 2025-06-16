using System;

[Serializable]
public class StageDTO
{
    public int StageNumber { get;}
    public string StageName { get;}
    public int CurrentLoopCount { get; }
    public bool HasEnteredBranchingSelection { get; }
    public bool IsReadyToAdvance { get; }
    public string NextDeterminedStageName { get; }
    public NextStageRoutingType NextStageRoutingType { get; }

    public StageDTO() { }

    public StageDTO(Stage stage)
    {
        StageNumber = stage.StageNumber;
        StageName = stage.StageName;
        CurrentLoopCount = stage.CurrentLoopCount;
        HasEnteredBranchingSelection = stage.HasEnteredBranchingSelection;
        IsReadyToAdvance = stage.IsReadyToAdvance;
        NextDeterminedStageName = stage.NextDeterminedStageConfig?.StageName ?? "None";
        NextStageRoutingType = stage.NextStageRoutingType;
    }
}
