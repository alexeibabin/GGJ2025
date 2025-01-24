using System;
using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Spawner
{
    [Serializable]
    public class SpawnableWrapper
    {
        [SerializeField] private GameObject spawnablePrefab;
        private const float FADE_IN_DURATION = 0.25f;
        private const float FADE_OUT_DURATION = 0.25f;

        public void Spawn(Vector3 position = default)
        {
            if (spawnablePrefab != null)
            {
                var instance = Object.Instantiate(spawnablePrefab, position, Quaternion.identity);
                FadeIn(instance);
                Game.EventHub.Notify(new SpawnInEvent(instance));
            }
            else
            {
                JamLogger.LogWarning("SpawnableWrapper is missing a prefab reference!");
            }
        }

        public void Destroy(GameObject instance)
        {
            if (instance != null)
            {
                FadeOut(instance, () =>
                {
                    Game.EventHub.Notify(new SpawnOutEvent(instance));
                    Object.Destroy(instance);
                });
            }
            else
            {
                JamLogger.LogWarning("Cannot destroy a null instance!");
            }
        }

        private void FadeIn(GameObject instance)
        {
            if (instance.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                Color color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
                spriteRenderer.DOFade(1f, FADE_IN_DURATION);
            }
            else
            {
                JamLogger.LogWarning("Cannot fade in a game object without a sprite renderer!");
            }
        }

        private void FadeOut(GameObject instance, Action onComplete)
        {
            if (instance.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.DOFade(0f, FADE_OUT_DURATION).OnComplete(() => onComplete?.Invoke());
            }
            else
            {
                onComplete?.Invoke();
            }
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