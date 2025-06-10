using System.Collections.Generic;
using UnityEngine;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        AchievementManager.OnInitialized += OnAchievementInitialized;
    }

    private void OnDestroy()
    {
        AchievementManager.OnInitialized -= OnAchievementInitialized;
    }

    private void OnAchievementInitialized(List<AchievementDTO> achievements)
    {
        BuildSlots(achievements);
    }
    public void BuildSlots(List<AchievementDTO> achievements)
    {
        foreach (Transform child in _slotParent)
            Destroy(child.gameObject);

        foreach (var achievement in achievements)
        {
            GameObject go = Instantiate(_slotPrefab, _slotParent);
            go.GetComponent<AchievementSlotUI>().SetData(achievement);
        }
        canvasGroup.alpha = 0f;
    }
}
