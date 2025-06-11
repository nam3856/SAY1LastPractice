using UnityEngine;

public class SaveManager
{
    private CurrencyRepository _currencyRepository = new CurrencyRepository();
    private AchievementRepository _achievementRepository = new AchievementRepository();
    private MockAttendanceRepository _attendanceRepository = new MockAttendanceRepository();

    private string _accountID;

    public SaveManager(string accountID = null)
    {
        _accountID = accountID;
    }
    public void LoadCurrencyData()
    {
        var currencyDTO = _currencyRepository.Load(_accountID);
        CurrencyManager.Instance.Initialize(currencyDTO);
    }

    public void LoadAchievementData()
    {
        var achieveDTO = _achievementRepository.Load(_accountID);
        AchievementManager.Instance.Initialize(achieveDTO);
    }

    public void SaveCurrencyData()
    {
        var dtos = CurrencyManager.Instance.GetAllCurrencyDTOs();
        _currencyRepository.Save(dtos, _accountID);
    }

    public void SaveAchievementData()
    {
        var dtos = AchievementManager.Instance.GetAllAchievementDTOs();
        _achievementRepository.Save(dtos, _accountID);
    }

    public void LoadAttendanceData()
    {
        var saved = _attendanceRepository.Load(_accountID);
        if (saved.HasValue)
        {
            AttendanceManager.Instance.LoadFromSaveModel(saved.Value);
        }
        else
        {
            AttendanceManager.Instance.Initialize();
        }

        AttendanceManager.Instance.CheckTodayAttendance();
    }

    public void SaveAttendanceData()
    {
        var data = AttendanceManager.Instance.ToSaveModel();
        _attendanceRepository.Save(data, _accountID);
    }
}