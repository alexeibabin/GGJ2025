using System.Collections.Generic;
using System.Globalization;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class SpawnConductor : MonoBehaviour
    {
        [SerializeField] private List<LevelSpawnInventory> levelSpawnInventory;

        private void Start()
        {
            Game.EventHub.Subscribe<TransitionStartedEvent>(OnTransitionStarted);
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