using Gpm.Ui;
using UnityEngine;

public class AchievementInfiniteScrollItem : InfiniteScrollItem
{
    private AchievementSlotUI _achievementSlotUI;

    private void Awake()
    {
        _achievementSlotUI = GetComponent<AchievementSlotUI>();
        if (_achievementSlotUI == null)
        {
            Debug.LogError("AchievementInfiniteScrollItem requires an AchievementSlotUI component.");
        }
    }

    public override void UpdateData(InfiniteScrollData data)
    {
        if (data is AchievementScrollData achievementData && _achievementSlotUI != null)
        {
            _achievementSlotUI.SetData(achievementData.DTO);
        }
    }
}
