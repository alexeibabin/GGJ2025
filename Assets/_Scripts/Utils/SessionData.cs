using System.Collections.Generic;
using _Scripts.Collectables;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class SessionData
{
    public Observable<float> BubbleHealth;
    public float ProgressTimer;
    public float TimeSinceLastTransition;
    public int TransitionsCompleted;
    public bool IsPaused;
    public int CurrentLevel;

    public ReactiveDictionary<ECollectableType, int> CollectedItems { get; set; } = new();
    public ReactiveDictionary<int, List<GameObject>> Spawned { get; set; } = new();

    public SessionData()
    {
        ResetSessionData();
    }

    public void ResetSessionData()
    {
        BubbleHealth.value = 1;
        ProgressTimer = 0;
        TimeSinceLastTransition = 0;
        TransitionsCompleted = 0;
        CurrentLevel = 0;
        ClearCollectables();
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
        if (CollectedItems.TryGetValue(collectableType, out var collectableCount))
        {
            return collectableCount;
        }
        else
        {
            return 0;
        }
    }

    private void ClearCollectables()
    {
        CollectedItems.Clear();
    }
}