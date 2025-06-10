using UnityEngine;
using System.Collections.Generic;
using Gpm.Ui;

public class AchievementScrollData : InfiniteScrollData
{
    public AchievementDTO DTO { get; private set; }

    public AchievementScrollData(AchievementDTO dto)
    {
        DTO = dto;
    }
}