using TMPro;
using UnityEngine;

public class UI_RankingEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetData(string rank, string name, string score)
    {
        rankText.text = rank;
        nameText.text = name;
        scoreText.text = score;
    }
}
