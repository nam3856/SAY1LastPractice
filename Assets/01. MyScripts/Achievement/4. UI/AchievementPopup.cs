using TMPro;
using UnityEngine;
using System.Collections;
public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetData(AchievementDTO dto)
    {
        _text.text = $"[업적 달성] {dto.Name}";
    }

}
