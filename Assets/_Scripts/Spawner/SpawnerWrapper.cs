using System;
using _Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Spawner
{
    [Serializable]
    public class SpawnableWrapper
    {
        [SerializeField] private GameObject spawnablePrefab;

        public void Spawn(Vector3 position = default)
        {
            if (spawnablePrefab != null)
            {
                // TODO: Spawn the prefab at the given position if needed
                Object.Instantiate(spawnablePrefab, spawnablePrefab.transform.position, Quaternion.identity);
            }
            else
            {
                JamLogger.LogWarning("SpawnableWrapper is missing a prefab reference!");
            }
        }
    }
}