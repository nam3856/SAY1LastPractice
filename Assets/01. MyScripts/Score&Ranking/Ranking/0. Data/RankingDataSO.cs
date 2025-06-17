using UnityEngine;

[CreateAssetMenu(fileName = "RankingDataSO", menuName = "Scriptable Objects/RankingDataSO")]
public class RankingDataSO : ScriptableObject
{
    public int Rank;
    public string PlayerName;
    public int Score;
}
