using System.Collections.Generic;
using System.Globalization;
using _Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class SpawnConductor : MonoBehaviour
    {
        [Header("Initial spawnable settings")]
        [Required] public SpawnableWrapper spawnable;
        public Vector3 spawnPosition;
        
        [Space]
        [SerializeField] private List<LevelSpawnInventory> levelSpawnInventory;

        private void Start()
        {
            Game.EventHub.Subscribe<TransitionStartedEvent>(OnTransitionStarted);
            Game.EventHub.Subscribe<ResetEvent>(OnGameReset);

            PlaceDefaultCollectible();
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
            
            PlaceDefaultCollectible();
        }

        private void OnTransitionStarted(TransitionStartedEvent evt)
        {
            var progressTimeAsInt = Mathf.FloorToInt(evt.progressTimer);

            foreach (var spawnData in levelSpawnInventory[Game.SessionData.CurrentLevel].Spawns)
            {
                if (spawnData.spawnTime == progressTimeAsInt)
                {
                    spawnData.spawnable.Spawn();
                }
                
                if (spawnData.despawnTime == progressTimeAsInt)
                {
                    spawnData.spawnable.Despawn();
                }
            }

            JamLogger.LogInfo(progressTimeAsInt.ToString(CultureInfo.InvariantCulture));
        }
    }
}