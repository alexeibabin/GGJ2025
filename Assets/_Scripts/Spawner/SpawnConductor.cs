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
    
    public struct EndGameStartedEvent : IEvent
    {
    }

    public class SpawnConductor : MonoBehaviour
    {
        [Header("Initial spawnable settings")]
        [Required] public SpawnableWrapper spawnable;
        public Vector3 spawnPosition;
        
        [Space]
        [SerializeField] private List<LevelSpawnInventory> levelSpawnInventory;
        
        private bool _isSpawning;

        private void Start()
        {
            Game.EventHub.Subscribe<GameTimerStartEvent>(evt => _isSpawning = true);
            Game.EventHub.Subscribe<AttemptSpawnEvent>(AttemptSpawnObstacles);
            Game.EventHub.Subscribe<ResetEvent>(OnGameReset);

            PlaceDefaultCollectible();
        }

        private void AttemptSpawnObstacles(AttemptSpawnEvent evt)
        {
            if (!_isSpawning) return;
            
            var progressTimeAsInt = Mathf.FloorToInt(Game.SessionData.TimeSinceLastTransition);
            
            JamLogger.LogInfo($"Attempting to spawn for time: {progressTimeAsInt}");
            
            if (Game.SessionData.TransitionsCompleted >= levelSpawnInventory.Count)
            {
                PlayerPrefs.SetInt(ProjectConstants.SAVED_COINS, Game.SessionData.GetCollectableCount(ECollectableType.Coin));
                PlayerPrefs.Save();
                JamLogger.LogWarning("No more levels to spawn!");
                Game.EventHub.Notify(new EndGameStartedEvent());
                _isSpawning = false;
                
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
            DestroyAllSpawnables();

            if (Game.SessionData.GetCollectableCount(ECollectableType.Coin) > 0)
            {
                PlaceDefaultCollectible();
            }
            
            _isSpawning = false;
        }
        
        public void DestroyAllSpawnables()
        {
            var allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            var spawnables = new List<ISpawnable>();

            foreach (var obj in allObjects)
            {
                var spawnable = obj.GetComponent<ISpawnable>();
                
                if (spawnable != null)
                {
                    spawnables.Add(spawnable);
                }
            }

            foreach (var spawnable in spawnables)
            {
                spawnable.Despawn(() => Object.Destroy(((MonoBehaviour)spawnable).gameObject));
            }
        }
    }
}