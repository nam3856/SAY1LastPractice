using System;
using UnityEngine;
public enum ECurrencyType
{
    Gold,
    Diamond,
}
public class Currency
{
    // 도메인 클래스의 장점:
    // 1. 표현력이 증가한다.
    // -> 화폐의 종류와 값을 명확하게 표현할 수 있다.
    // 2. 무결성이 유지된다. (무결성: 데이터의 일관성과 정확성, 유효성을 유지하는 것)
    // 3. 데이터와 데이터를 다루는 로직이 뭉쳐있다. -> 응집도가 높다.

    // 자기 서술적인 코드가 된다. (기획서에 의거한 코드가 된다.)
    // 도메인(기획서) 변경이 일어나면 코드에 반영하기 쉽다.


    // 도메인 모델: Currency (콘텐츠, 지식, 문제, 기획서를 바탕으로 작성한다: 기획자랑 말이 통해야한다.)

    private ECurrencyType _type;
    public ECurrencyType Type => _type;
    private int _value = 0;
    public int Value => _value;

    // 도메인은 '규칙'이 있다.
    public Currency(ECurrencyType type, int initialValue = 0)
    {
        if(initialValue < 0)
        {
            throw new Exception("초기값은 0 이상이어야 합니다.");
        }
        _type = type;
        _value = initialValue;
    }

    public void Add(int amount)
    {
        if (amount < 0)
        {
            throw new Exception("추가할 금액은 0 이상이어야 합니다.");
        }
        _value += amount;
    }

    public void Subtract(int amount)
    {
        if (amount < 0)
        {
            throw new Exception("차감할 금액은 0 이상이어야 합니다.");
        }
        if (_value < amount)
        {

            throw new Exception("차감할 금액이 현재 금액보다 많습니다.");
        }
        _value -= amount;
    }
    public override string ToString()
    {
        return $"{_type}: {_value}";
    }

    public bool TrySpend(int amount)
    {
        if (_value < amount)
            return false;

        _value -= amount;
        return true;
    }
}