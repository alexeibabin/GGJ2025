using System.Collections.Generic;
using _Scripts.Collectables;

public class SessionData
{
    public float BubbleHealth;
    public float ProgressTimer;
    public float TimeSinceLastTransition;
    public bool IsPaused;

    private Dictionary<ECollectableType, int> CollectedItems { get; set; } = new();

    public SessionData()
    {
        BubbleHealth = 1;
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
        return CollectedItems.GetValueOrDefault(collectableType, 0);
    }

    private void ClearCollectables()
    {
        CollectedItems.Clear();
    }
}