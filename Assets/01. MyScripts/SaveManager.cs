using UnityEngine;

public class SaveManager
{
    private CurrencyRepository _currencyRepository = new CurrencyRepository();
    private AchievementRepository _achievementRepository = new AchievementRepository();

    public void LoadCurrencyData()
    {
        var currencyDTO = _currencyRepository.Load();
        CurrencyManager.Instance.Initialize(currencyDTO);
    }

    public void LoadAchievementData()
    {
        var achieveDTO = _achievementRepository.Load();
        AchievementManager.Instance.Initialize(achieveDTO);
    }

    public void SaveCurrencyData()
    {
        var dtos = CurrencyManager.Instance.GetAllCurrencyDTOs();
        _currencyRepository.Save(dtos);
    }

    public void SaveAchievementData()
    {
        var dtos = AchievementManager.Instance.GetAllAchievementDTOs();
        _achievementRepository.Save(dtos);
    }
}