using System;
using System.Collections.Generic;
using _Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Spawner
{
    [CreateAssetMenu(fileName = "LevelSpawnInventory", menuName = "Spawner/LevelSpawnInventory")]
    public class LevelSpawnInventory : ScriptableObject
    {
        [Serializable]
        public class SpawnData
        {
            [Required] public SpawnableWrapper spawnable;
            [MinValue(0)] public int spawnTime;
        }

        [TableList]
        [SerializeField] private List<SpawnData> spawns = new List<SpawnData>();

        public List<SpawnData> Spawns => spawns;

        [Button(ButtonSizes.Large), GUIColor(0.1f, 0.8f, 0.1f)]
        private void DuplicateLastSpawn()
        {
            if (spawns.Count == 0)
            {
                JamLogger.LogWarning("No spawns found to duplicate!");
                return;
            }

            var lastSpawn = spawns[^1];
            
            if (lastSpawn != null)
            {
                var duplicate = new SpawnData
                {
                    spawnable = lastSpawn.spawnable,
                    spawnTime = lastSpawn.spawnTime
                };

                spawns.Add(duplicate);
                JamLogger.LogInfo("Duplicated the last spawn entry.");
            }
        }
    }
}