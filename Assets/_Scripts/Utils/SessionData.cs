using System.Collections.Generic;
using _Scripts.Collectables;
using UniRx;
using UnityEngine;

public class SessionData
{
    public float BubbleHealth;
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
        BubbleHealth = 1;
        ProgressTimer = 0;
        TimeSinceLastTransition = 0;
        ClearCollectables();
        TransitionsCompleted = 0;
        CurrentLevel = 0;
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

    public void AddSpawnable(GameObject evtGameObject)
    {
        if (!Spawned.ContainsKey(CurrentLevel))
        {
            Spawned.Add(CurrentLevel, new List<GameObject>());
        }
        
        Spawned[CurrentLevel].Add(evtGameObject);
    }

    public void RemoveSpawnable(GameObject evtGameObject)
    {
        if (Spawned.ContainsKey(CurrentLevel))
        {
            Spawned[CurrentLevel].Remove(evtGameObject);
        }
    }
    
    public void ClearSpawnables()
    {
        if (Spawned.ContainsKey(CurrentLevel))
        {
            foreach (var spawnable in Spawned[CurrentLevel])
            {
                Object.Destroy(spawnable);
            }
            
            Spawned[CurrentLevel].Clear();
        }
    }
}