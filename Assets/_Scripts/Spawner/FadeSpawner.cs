using System;
using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class FadeSpawner : MonoBehaviour, ISpawnable
    {
        private const float FADE_IN_DURATION = 0.25f;
        private const float FADE_OUT_DURATION = 0.25f;
        
        public void Spawn()
        {
            SpriteRenderer renderer = null;
            
            if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                renderer = spriteRenderer;
            }
            else if (gameObject.TryGetComponentInChildren<SpriteRenderer>(out var childSpriteRenderer))
            {
                renderer = childSpriteRenderer;
            }
            else
            {
                JamLogger.LogWarning("Cannot fade in a game object without a sprite renderer!");
                return;
            }
            
            var color = renderer.color;
            color.a = 0;
            renderer.color = color;
            renderer.DOFade(1f, FADE_IN_DURATION);
        }

        public void Despawn(Action onComplete)
        {
            if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.DOFade(0f, FADE_OUT_DURATION).OnComplete(() => onComplete?.Invoke());
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}