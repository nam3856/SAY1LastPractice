public class DifficultyDTO
{
    public float CurrentElapsedTime { get; }
    public float CurrentCoefficient { get; }
    public string CurrentTierName { get; }

    public DifficultyDTO() { }

    public DifficultyDTO(float currentElapsedTime, float currentCoefficient, string currentTierName)
    {
        CurrentElapsedTime = currentElapsedTime;
        CurrentCoefficient = currentCoefficient;
        CurrentTierName = currentTierName;
    }
}
