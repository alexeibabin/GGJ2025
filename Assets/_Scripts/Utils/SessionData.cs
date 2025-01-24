using System.Collections.Generic;
using _Scripts.Collectables;
using UniRx;

public class SessionData
{
    public float BubbleHealth;
    public float ProgressTimer;
    public float TimeSinceLastTransition;
    public int TransitionsCompleted;
    public bool IsPaused;

    public ReactiveDictionary<ECollectableType, int> CollectedItems { get; set; } = new();

    public SessionData()
    {
        ResetSessionData();
    }

    public void ResetSessionData()
    {
        BubbleHealth = 1;
        ProgressTimer = 0;
        TimeSinceLastTransition = 0;
        ClearCollectables();
        TransitionsCompleted = 0;
    }
    
    public void AddCollectable(ECollectableType collectableType)
    {
        if (!CollectedItems.TryAdd(collectableType, 1))
        {
            CollectedItems[collectableType]++;
        }
    }

    public int GetCollectableCount(ECollectableType collectableType)
    {
        if (CollectedItems.ContainsKey(collectableType))
        {
            return CollectedItems[collectableType];
        }
        else
        {
            return 0;
        }
    }

    public void ClearCollectables()
    {
        CollectedItems.Clear();
    }
}