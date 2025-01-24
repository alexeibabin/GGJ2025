using System.Globalization;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class SpawnConductor : MonoBehaviour
    {
        [SerializeField] private LevelSpawnInventory levelSpawnInventory;

        private void Start()
        {
            Game.EventHub.Subscribe<TransitionStartedEvent>(OnTransitionStarted);
        }

        private void OnTransitionStarted(TransitionStartedEvent evt)
        {
            var progressTimeAsInt = Mathf.FloorToInt(evt.progressTimer);

            foreach (var spawnData in levelSpawnInventory.Spawns)
            {
                if (spawnData.spawnTime == progressTimeAsInt)
                {
                    spawnData.spawnable.Spawn();
                }
            }

            JamLogger.LogInfo(progressTimeAsInt.ToString(CultureInfo.InvariantCulture));
        }

    }
}