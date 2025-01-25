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
                Game.EventHub.Notify(new SpawnInEvent(_instance));
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
                spawnable.Despawn(() =>
                {
                    Game.EventHub.Notify(new SpawnOutEvent(spawnableObject));
                    Object.Destroy(spawnableObject);
                });
            }
            else
            {
                JamLogger.LogWarning("Cannot destroy a null instance!");
            }
        }


        public struct SpawnInEvent : IEvent
        {
            public GameObject gameObject;

            public SpawnInEvent(GameObject instance)
            {
                gameObject = instance;
            }
        }

        public struct SpawnOutEvent : IEvent
        {
            public GameObject gameObject;

            public SpawnOutEvent(GameObject instance)
            {
                gameObject = instance;
            }
        }
    }
}