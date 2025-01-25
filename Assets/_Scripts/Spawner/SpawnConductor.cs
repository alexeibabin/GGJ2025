using System.Collections.Generic;
using _Scripts.Collectables;
using _Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Spawner
{
    public struct AttemptSpawnEvent : IEvent
    {
    }

    public class SpawnConductor : MonoBehaviour
    {
        [Header("Initial spawnable settings")]
        [Required] public SpawnableWrapper spawnable;
        public Vector3 spawnPosition;
        
        [Space]
        [SerializeField] private List<LevelSpawnInventory> levelSpawnInventory;

        private void Start()
        {
            Game.EventHub.Subscribe<AttemptSpawnEvent>(AttemptSpawnObstacles);
            Game.EventHub.Subscribe<ResetEvent>(OnGameReset);

            PlaceDefaultCollectible();
        }

        private void AttemptSpawnObstacles(AttemptSpawnEvent evt)
        {
            var progressTimeAsInt = Mathf.FloorToInt(Game.SessionData.TimeSinceLastTransition);
            
            JamLogger.LogInfo($"Attempting to spawn for time: {progressTimeAsInt}");
            
            if (Game.SessionData.TransitionsCompleted >= levelSpawnInventory.Count)
            {
                JamLogger.LogWarning("No more levels to spawn!");
                return;
            }

            foreach (var spawnData in levelSpawnInventory[Game.SessionData.TransitionsCompleted].Spawns)
            {
                if (spawnData.spawnTime == progressTimeAsInt)
                {
                    JamLogger.LogInfo("Spawning at time: " + progressTimeAsInt + " for level: " + Game.SessionData.TransitionsCompleted + " with position: " + spawnData.spawnPosition);
                    spawnData.spawnable.Spawn(spawnData.spawnPosition);
                }
                
                if (spawnData.despawnTime == progressTimeAsInt)
                {
                    spawnData.spawnable.Despawn();
                }
            }
        }

        private void PlaceDefaultCollectible()
        {
            spawnable.Spawn(spawnPosition);
        }

        private void OnGameReset(ResetEvent evt)
        {
            foreach (var spawnData in levelSpawnInventory[Game.SessionData.CurrentLevel].Spawns)
            {
                spawnData.spawnable.Despawn();
            }

            if (Game.SessionData.GetCollectableCount(ECollectableType.Coin) > 0)
            {
                PlaceDefaultCollectible();
            }
        }
    }
}