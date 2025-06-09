using System;
using System.Collections.Generic;
using UnityEngine;

// 아키텍쳐: 설계 그 자체(설계마다 철학이 있다.)
// 디자인 패턴: 설계를 구현하는 과정에서 쓰이는 패턴

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public event Action CurrenciesChanged;
    public IReadOnlyDictionary<ECurrencyType, Currency> Currencies => _currencies;

    private static readonly ECurrencyType[] _currencyTypes = 
        (ECurrencyType[])Enum.GetValues(typeof(ECurrencyType));
    private Dictionary<ECurrencyType, Currency> _currencies = 
        new Dictionary<ECurrencyType, Currency>();
    
    private CurrencyRepository _repository;

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
        Init();
    }

    private void Init()
    {
        // 레포지토리
        _repository = new CurrencyRepository();

        // 레포지토리에서 데이터 불러오기
        List<CurrencyDTO> loadedData = _repository.Load();
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
        
    }

    public void Add(ECurrencyType type, int amount)
    {
        _currencies[type].Add(amount);

        CurrenciesChanged?.Invoke(); // 이벤트 호출

        _repository.Save(_currencies.ToDtoList()); // 레포지토리에 데이터 저장
    }

    public void Subtract(ECurrencyType type, int amount)
    {
        _currencies[type].Subtract(amount);

        CurrenciesChanged?.Invoke(); // 이벤트 호출
        Debug.Log($"{type} 차감: {amount}, 현재 금액: {_currencies[type].Value}");
    }

    public List<CurrencyDTO> GetAllCurrencyDTOs()
    {
        return _currencies.ToDtoList();
    }

    public bool TrySpend(ECurrencyType type, int amount)
    {
        var currency = _currencies[type];
        if (!currency.TrySpend(amount))
            return false;

        CurrenciesChanged?.Invoke();

        _repository.Save(_currencies.ToDtoList()); // 레포지토리에 데이터 저장

        return true;
    }
}