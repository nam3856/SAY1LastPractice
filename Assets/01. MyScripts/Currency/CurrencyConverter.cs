using System.Collections.Generic;

public static class CurrencyConverter
{
    public static List<CurrencyDTO> ToDtoList(this Dictionary<ECurrencyType, Currency> currencies)
    {
        var list = new List<CurrencyDTO>();
        foreach (var pair in currencies)
        {
            list.Add(new CurrencyDTO(pair.Value));
        }
        return list;
    }
}