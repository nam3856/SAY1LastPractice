using System;
using System.Collections.Generic;
using UnityEngine;

// 아키텍쳐: 설계 그 자체(설계마다 철학이 있다.)
// 디자인 패턴: 설계를 구현하는 과정에서 쓰이는 패턴

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public event Action CurrenciesChanged;
    private static readonly ECurrencyType[] _currencyTypes = 
        (ECurrencyType[])Enum.GetValues(typeof(ECurrencyType));

    private Dictionary<ECurrencyType, Currency> _currencies = 
        new Dictionary<ECurrencyType, Currency>();
    public IReadOnlyDictionary<ECurrencyType, Currency> Currencies => _currencies;

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
        // 생성
        for (int i = 0; i < _currencyTypes.Length; i++)
        {
            ECurrencyType type = _currencyTypes[i];

            Currency currency = new Currency(type, 0);
            _currencies.Add(type, currency);
        }
    }

    public void Add(ECurrencyType type, int amount)
    {
        _currencies[type].Add(amount);

        CurrenciesChanged?.Invoke(); // 이벤트 호출
        Debug.Log($"{type} 추가: {amount}, 현재 금액: {_currencies[type].Value}");
    }

    public void Subtract(ECurrencyType type, int amount)
    {
        _currencies[type].Subtract(amount);

        CurrenciesChanged?.Invoke(); // 이벤트 호출
        Debug.Log($"{type} 차감: {amount}, 현재 금액: {_currencies[type].Value}");
    }

    public List<CurrencyDTO> GetAllCurrencyDTOs()
    {
        var list = new List<CurrencyDTO>();
        foreach (var pair in _currencies)
        {
            list.Add(new CurrencyDTO(pair.Value));
        }
        return list;
    }

    public bool TrySpend(ECurrencyType type, int amount)
    {
        var currency = _currencies[type];
        if (!currency.TrySpend(amount))
            return false;

        CurrenciesChanged?.Invoke();
        return true;
    }
}