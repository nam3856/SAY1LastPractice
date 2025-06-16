using System;
using System.Collections.Generic;
using UnityEngine;

// 아키텍쳐: 설계 그 자체(설계마다 철학이 있다.)
// 디자인 패턴: 설계를 구현하는 과정에서 쓰이는 패턴

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public IReadOnlyDictionary<ECurrencyType, Currency> Currencies => _currencies;

    private static readonly ECurrencyType[] _currencyTypes = 
        (ECurrencyType[])Enum.GetValues(typeof(ECurrencyType));
    private Dictionary<ECurrencyType, Currency> _currencies = 
        new Dictionary<ECurrencyType, Currency>();
    

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

    private void Start()
    {
        GameManager.Instance.Events.Achievement.OnAchievementClaimed += OnAchievementClaimed;
        GameManager.Instance.Events.Attendance.OnAttendanceRewardClaimed += OnAttendanceRewardClaimed;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Events.Achievement.OnAchievementClaimed -= OnAchievementClaimed;
            GameManager.Instance.Events.Attendance.OnAttendanceRewardClaimed -= OnAttendanceRewardClaimed;
        }
        
    }

    private void OnAttendanceRewardClaimed(AttendanceRewardDTO attendanceData)
    {
        Add(attendanceData.RewardCurrencyType, attendanceData.RewardCurrencyAmount);
        Debug.Log($"출석 보상 지급 완료: {attendanceData.RewardCurrencyType} {attendanceData.RewardCurrencyAmount}");
    }
    private void OnAchievementClaimed(AchievementDTO achievement)
    {
        Add(achievement.RewardCurrencyType, achievement.RewardCurrencyAmount);
        Debug.Log($"업적 보상 지급 완료: {achievement.RewardCurrencyType} {achievement.RewardCurrencyAmount}");
    }

    public void Initialize(List<CurrencyDTO> loadedData)
    {
        _currencies.Clear();
        if (loadedData != null && loadedData.Count > 0)
        {
            // 로드된 데이터로 초기화
            foreach (var dto in loadedData)
            {
                if (_currencies.ContainsKey(dto.CurrencyType))
                {
                    _currencies[dto.CurrencyType].Add(dto.Value);
                }
                else
                {
                    _currencies.Add(dto.CurrencyType, new Currency(dto.CurrencyType, dto.Value));
                }
            }
        }
        else
        {
            for (int i = 0; i < _currencyTypes.Length; i++)
            {
                ECurrencyType type = _currencyTypes[i];

                Currency currency = new Currency(type, 0);
                _currencies.Add(type, currency);
            }
        }


        InitManager.Instance.ReportInitialized("Currency");

    }

    public void Add(ECurrencyType type, int amount)
    {
        _currencies[type].Add(amount);

        GameManager.Instance.Events.Currency.RaiseCurrencyChanged(type, _currencies[type].Value);

        GameManager.Instance.SaveRequested(); 
    }

    public void OnInitialized()
    {
        foreach(var type in _currencyTypes)
        {
            if (_currencies.ContainsKey(type))
            {
                GameManager.Instance.Events.Currency.RaiseCurrencyChanged(type, _currencies[type].Value);
                Debug.Log($"{type}, {_currencies[type].Value}");
            }
        }
        
    }

    //public void Subtract(ECurrencyType type, int amount)
    //{
    //    _currencies[type].Subtract(amount);

    //    GameManager.Instance.Events.Currency.RaiseCurrencyChanged(type, _currencies[type].Value);
    //    Debug.Log($"{type} 차감: {amount}, 현재 금액: {_currencies[type].Value}");
    //}

    public List<CurrencyDTO> GetAllCurrencyDTOs()
    {
        return _currencies.ToDtoList();
    }

    public bool TrySpend(ECurrencyType type, int amount)
    {
        var currency = _currencies[type];
        if (!currency.TrySpend(amount))
            return false;

        GameManager.Instance.Events.Currency.RaiseCurrencyChanged(type, _currencies[type].Value);
        GameManager.Instance.SaveRequested(); 

        return true;
    }
}