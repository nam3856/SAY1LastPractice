using NUnit.Framework;
using System;

public class RankingEventManager
{
    public event Action OnRankingUpdated;

    public void RaiseRankingUpdated()
    {
        OnRankingUpdated?.Invoke();
    }
}