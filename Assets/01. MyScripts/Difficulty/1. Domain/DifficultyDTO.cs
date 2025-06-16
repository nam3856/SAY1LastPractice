public class DifficultyDTO
{
    public float CurrentElapsedTime { get; }
    public float CumulativeStageMultiplier { get; }
    public float CurrentCoefficient { get; }
    public string CurrentTierName { get; }

    public DifficultyDTO() { }

    public DifficultyDTO(float currentElapsedTime, float cumulativeStageMultiplier, float currentCoefficient, string currentTierName)
    {
        CurrentElapsedTime = currentElapsedTime;
        CumulativeStageMultiplier = cumulativeStageMultiplier;
        CurrentCoefficient = currentCoefficient;
        CurrentTierName = currentTierName;
    }
}
