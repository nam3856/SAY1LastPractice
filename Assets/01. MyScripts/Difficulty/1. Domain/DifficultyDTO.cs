public class DifficultyDTO
{
    public float CurrentElapsedTime { get; }
    public float CurrentCoefficient { get; }
    public string CurrentTierName { get; }
    public EDifficulty CurrentTierEnum { get; }

    public DifficultyDTO() { }

    public DifficultyDTO(float currentElapsedTime, float currentCoefficient, string currentTierName, EDifficulty currentTierEnum)
    {
        CurrentElapsedTime = currentElapsedTime;
        CurrentCoefficient = currentCoefficient;
        CurrentTierName = currentTierName;
        CurrentTierEnum = currentTierEnum;
    }
}
