using UnityEngine;

// 계층 간 데이터 전송을 위해 도메인 모델 대신 사용되는 DTO(Data Transfer Object) 클래스입니다.
[System.Serializable]
public class CurrencyDTO
{
    public readonly ECurrencyType CurrencyType;
    public readonly int Value;

    public CurrencyDTO(Currency currency)
    {
        CurrencyType = currency.Type;
        Value = currency.Value;
    }

    public bool HaveEnough(int amount)
    {
        return Value >= amount;
    }
}
