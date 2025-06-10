using UnityEngine;

public class AchievementNotifyUI : MonoBehaviour
{
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private Transform _popupParent;

    private void Start()
    {
        GameManager.Instance.Events.Achievement.OnAchievementUnlocked += ShowPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.Achievement.OnAchievementUnlocked -= ShowPopup;
    }

    private void ShowPopup(AchievementDTO dto)
    {
        GameObject go = Instantiate(_popupPrefab, _popupParent);
        go.GetComponent<AchievementPopup>().SetData(dto);
    }
}