using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class HazardSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabToSpawn;

        [UsedImplicitly] // invoked via animation event
        private void SpawnPrefab()
        {
            Instantiate(_prefabToSpawn, transform.position, transform.rotation, transform);
        }
    }
}
