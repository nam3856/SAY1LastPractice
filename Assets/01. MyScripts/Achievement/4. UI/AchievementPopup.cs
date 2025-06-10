using TMPro;
using UnityEngine;
using System.Collections;
public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _duration = 3f;

    public void SetData(AchievementDTO dto)
    {
        _text.text = $"[업적 달성] {dto.Name}";
        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }
}
