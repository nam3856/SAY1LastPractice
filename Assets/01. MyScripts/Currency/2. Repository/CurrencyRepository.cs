using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Repository: 데이터의 영속성을 보장한다.
// 영속성: 프로그램을 종료해도 데이터가 보존되는 것
// Save / Load

public class CurrencyRepository
{
    private const string SAVE_KEY = nameof(CurrencyRepository);
    public void Save(List<CurrencyDTO> dtos)
    {
        CurrencySaveData saveData = new CurrencySaveData
        {
            DataList = dtos
        };
        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
    }
    public List<CurrencyDTO> Load() 
    {
        if(!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.LogWarning("No saved data found for CurrencyRepository.");
            return null;
        }
        string json = PlayerPrefs.GetString(SAVE_KEY);
        CurrencySaveData saveData = JsonUtility.FromJson<CurrencySaveData>(json);
        if (saveData == null || saveData.DataList == null)
        {
            Debug.LogWarning("Failed to load Currency data.");
            return null;
        }

        return saveData.DataList;
    }
}
[System.Serializable]
public class CurrencySaveData
{
    public List<CurrencyDTO> DataList;
}
