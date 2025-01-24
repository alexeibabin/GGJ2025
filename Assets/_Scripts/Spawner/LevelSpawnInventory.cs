using System;
using System.Collections.Generic;
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
            public SpawnableWrapper spawnable;
            public int spawnTime;
        }

        [TableList]
        [SerializeField] private List<SpawnData> spawns = new List<SpawnData>();

        public List<SpawnData> Spawns => spawns;
    }
}