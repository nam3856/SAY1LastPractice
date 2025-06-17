using UnityEngine;

public class SaveManager
{
    private CurrencyRepository _currencyRepository = new CurrencyRepository();
    private AchievementRepository _achievementRepository = new AchievementRepository();
    private AttendanceRepository _attendanceRepository = new AttendanceRepository();
    private ScoreRepository _scoreRepository = new ScoreRepository();
    private RankingRepository _rankingRepository = new RankingRepository();

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

    public void SaveScoreData()
    {
        var scoreData = ScoreManager.Instance.GetScoreDTO();
        _scoreRepository.Save(scoreData, _accountID);
    }

    public void LoadScoreData()
    {
        var scoreData = _scoreRepository.Load(_accountID);
        if (scoreData != null)
        {
            ScoreManager.Instance.Initialize(_accountID, scoreData);
            Debug.Log("점수 데이터 로드 완료");
        }
        else
        {
            Debug.Log("점수 데이터가 없습니다. 없이 초기화합니다.");
            ScoreManager.Instance.Initialize(_accountID);
        }
    }

    public void SaveRankingData()
    {
        var rankingList = RankingManager.Instance.GetTopRankings();
        _rankingRepository.Save(rankingList);
    }

    public void LoadRankingData()
    {
        var rankingList = _rankingRepository.Load();
        if (rankingList.Count > 0)
        {
            RankingManager.Instance.Initialize(rankingList);
            Debug.Log("랭킹 데이터 로드 완료");
        }
        else
        {
            RankingManager.Instance.Initialize();
            Debug.Log("랭킹 데이터가 없습니다. 초기화합니다.");
        }
    }
}