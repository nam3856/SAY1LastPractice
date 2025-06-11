using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private List<AchievementDataSO> _achievementDataList;
    public IReadOnlyList<Achievement> Achievements => _achievements;

    public static event Action<List<AchievementDTO>> OnInitialized;

    private List<Achievement> _achievements;
    public static AchievementManager Instance { get; private set; }

    private int _lastGoldValue = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Events.Currency.OnCurrencyChanged -= OnCurrencyChanged;
            GameManager.Instance.Events.Attendance.OnTodayAttendanceChecked -= OnAttendanceChecked;
        }
    }

    private void OnCurrencyChanged(CurrencyChangedEventArgs args)
    {
        if (args.CurrencyType != ECurrencyType.Gold) return;

        int delta = args.NewValue - _lastGoldValue;
        if (delta > 0)
        {
            Increase(EAchievementCondition.GoldCollect, delta);
        }

        _lastGoldValue = args.NewValue;
    }

    private void OnAttendanceChecked()
    {
        // 출석 체크 시 업적 진행도 증가
        Increase(EAchievementCondition.Attendance, 1);
    }


    public void Initialize(List<AchievementDTO> savedData)
    {
        _achievements = new List<Achievement>();
        GameManager.Instance.Events.Currency.OnCurrencyChanged += OnCurrencyChanged;
        GameManager.Instance.Events.Attendance.OnTodayAttendanceChecked += OnAttendanceChecked;

        var validIds = new HashSet<string>();
        foreach (var dataSO in _achievementDataList)
            validIds.Add(dataSO.ID);

        // 삭제된 업적 처리
        if (savedData != null)
        {
            foreach (var dto in savedData)
            {
                if (validIds.Contains(dto.ID) == false)
                {
                    Debug.LogWarning($"[업적 로드] '{dto.ID}'는 삭제된 업적으로 무시됩니다.");
                }
            }
        }

        foreach (var dataSO in _achievementDataList)
        {
            var match = savedData?.Find(dto => dto.ID == dataSO.ID);

            // 저장된 데이터가 있으면 해당 진행도와 수령 여부를 포함하여 업적을 복원하고,
            // 없으면 새로운 업적으로 초기화
            Achievement a = match != null ? new Achievement(dataSO, match) : new Achievement(dataSO);

            _achievements.Add(a);
        }

        // 골드 관련 진행도 저장된 값으로 초기화
        var goldAchievement = _achievements.Find(a => a.Condition == EAchievementCondition.GoldCollect);
        _lastGoldValue = goldAchievement?.CurrentValue ?? 0;

        OnInitialized?.Invoke(GetAllAchievementDTOs());

        InitManager.Instance.ReportInitialized("Achievement");
    }



    public void Increase(EAchievementCondition condition, int value)
    {
        foreach (var achievement in _achievements)
        {
            if (achievement.Condition == condition)
            {
                achievement.Increase(value);

                //업적 완료 확인
                if (achievement.CanClaimReward())
                {
                    if(achievement.IsUnlocked == false)
                    {
                        achievement.IsUnlocked = true; 
                        GameManager.Instance.Events.Achievement.RaiseAchievementUnlocked(new AchievementDTO(achievement));
                    }
                }

                GameManager.Instance.Events.Achievement.RaiseAchievementUpdated(new AchievementDTO(achievement));
            }
        }
    }

    public List<AchievementDTO> GetAllAchievementDTOs()
    {
        var list = new List<AchievementDTO>();
        foreach (var a in _achievements)
        {
            list.Add(new AchievementDTO(a));
        }
        return list;
    }

    public bool TryClaimReward(AchievementDTO achievementDTO)
    {
        Achievement achievement = _achievements.Find(a => a.ID == achievementDTO.ID);
        if (achievement == null)
        {
            Debug.LogError($"업적을 찾을 수 없습니다: {achievementDTO.ID}");
            return false;
        }

        if (achievement.TryClaim())
        {
            var updatedDTO = new AchievementDTO(achievement);
            GameManager.Instance.Events.Achievement.RaiseAchievementClaimed(updatedDTO);
            return true;
        }
        else
        {
            Debug.LogWarning($"업적 보상을 받을 수 없습니다: {achievementDTO.ID}");
            return false;
        }
    }
}