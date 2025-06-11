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

    public void Save(List<CurrencyDTO> dtos, string id)
    {
        // 변환
        var saveModels = new List<CurrencySaveModel>();
        foreach (var dto in dtos)
        {
            saveModels.Add(new CurrencySaveModel
            {
                CurrencyType = dto.CurrencyType,
                Value = dto.Value
            });
        }

        var saveData = new CurrencySaveData { DataList = saveModels };
        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(SAVE_KEY + "_" + id, json);
    }

    public List<CurrencyDTO> Load(string id)
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY + "_" +id))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY + "_" + id);
        var saveData = JsonUtility.FromJson<CurrencySaveData>(json);
        if (saveData?.DataList == null)
            return null;

        var dtos = new List<CurrencyDTO>();
        foreach (var model in saveData.DataList)
        {
            dtos.Add(new CurrencyDTO(model.CurrencyType, model.Value));
        }

        return dtos;
    }
}
[System.Serializable]
public struct CurrencySaveModel
{
    public ECurrencyType CurrencyType;
    public int Value;
}
[System.Serializable]
public class CurrencySaveData
{
    public List<CurrencySaveModel> DataList;
}