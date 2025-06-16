using System;

public class DifficultyEventManager
{
    public event Action<DifficultyDTO> OnTierChanged;

    public event Action<float> OnSliderChanged;

    public void RaiseTierChanged(DifficultyDTO dto)
    {
        OnTierChanged?.Invoke(dto);
    }

    public void RaiseSliderChanged(float coefficient)
    {
        OnSliderChanged?.Invoke(coefficient);
    }
}
