using UnityEngine;

public class SaveManager
{
    private CurrencyRepository _currencyRepository = new CurrencyRepository();
    private AchievementRepository _achievementRepository = new AchievementRepository();
    private AttendanceRepository _attendanceRepository = new AttendanceRepository();

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
        var dataBundle = _attendanceRepository.LoadAll();

        var savedSlots = dataBundle.Rewards;
        var savedAttendance = dataBundle.AttendanceDTO;

        if (savedSlots.Count>0 && savedAttendance != null)
        {
            AttendanceManager.Instance.LoadFromSaveModel(savedSlots, savedAttendance);
            Debug.Log("출석 데이터 로드 완료");
        }
        else
        {
            AttendanceManager.Instance.Initialize();
            Debug.Log("출석 데이터 초기화");
        }

    }

    public void SaveAttendanceData()
    {
        var slotsData = AttendanceManager.Instance.GetAttendanceSlotDTOs();
        var attendanceData = AttendanceManager.Instance.GetCurrentAttendanceDTO();
        _attendanceRepository.SaveAttendance(attendanceData);
        _attendanceRepository.SaveRewards(slotsData);

    }
}