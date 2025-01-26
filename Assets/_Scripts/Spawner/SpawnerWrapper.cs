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

        private GameObject _instance;

        public void Spawn(Vector3 position = default)
        {
            if (spawnablePrefab != null)
            {
                _instance = Object.Instantiate(spawnablePrefab, position, Quaternion.identity);
                var spawnable = _instance.GetComponent<ISpawnable>();
                spawnable.Spawn();
            }
            else
            {
                JamLogger.LogWarning("SpawnableWrapper is missing a prefab reference!");
            }
        }

        public void Despawn()
        {
            if (_instance != null)
            {
                AnimateAndDestroy(_instance);
            }
        }

        public void AnimateAndDestroy(GameObject spawnableObject)
        {
            var spawnable = spawnableObject.GetComponent<ISpawnable>();
            if (spawnable != null)
            {
                spawnable.Despawn(() => { Object.Destroy(spawnableObject); });
            }
            else
            {
                JamLogger.LogWarning("Cannot destroy a null instance!");
            }
        }
    }
}