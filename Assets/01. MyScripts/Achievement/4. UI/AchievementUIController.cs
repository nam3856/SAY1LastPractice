using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private InfiniteScroll scroll;

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
        if (scroll == null)
        {
            Debug.LogError("InfiniteScroll 컴포넌트가 할당되지 않았습니다.");
            return;
        }
        scroll.ClearData(); // 기존 데이터 초기화

        foreach (var dto in achievements)
        {
            var data = new AchievementScrollData(dto);
            scroll.InsertData(data);
        }
        canvasGroup.alpha = 0f;
    }
}
