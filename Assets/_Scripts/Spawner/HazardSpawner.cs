using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class HazardSpawner : MonoBehaviour, ISpawnable
    {
        [SerializeField] private GameObject _prefabToSpawn;
        private ISpawnable _spawnedObject;

        [UsedImplicitly] // invoked via animation event
        private void SpawnPrefab()
        {
            _spawnedObject = Instantiate(_prefabToSpawn, transform).GetComponent<ISpawnable>();
        }

        public void Spawn()
        {
            _spawnedObject?.Spawn();
        }

        public void Despawn(Action onComplete)
        {
            if (_spawnedObject != null)
            {
                _spawnedObject.Despawn(onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}
