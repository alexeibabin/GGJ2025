using System;
using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class FadeSpawner : MonoBehaviour, ISpawnable
    {
        [SerializeField] private float _fadeInDuration = 0.25f;
        [SerializeField] private float _fadeOutDuration = 1f;
        [SerializeField] private float _finalScale = 3f;
        
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
            renderer.DOFade(1f, _fadeInDuration);
        }

        public void Despawn(Action onComplete)
        {
            if (this == null) return;
            
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            var childRenderers = GetComponentsInChildren<SpriteRenderer>();

            if (gameObject.TryGetComponent(out Collider2D collider2D))
            {
                collider2D.enabled = false;
            }

            var sequence = DOTween.Sequence();
            
            if (spriteRenderer != null)
            {
                sequence.Join(spriteRenderer.DOFade(0f, _fadeOutDuration));
            }

            if (childRenderers != null)
            {
                foreach (var childRenderer in childRenderers)
                {
                    sequence.Join(childRenderer.DOFade(0f, _fadeOutDuration));
                }
            }
            
            sequence
                .Join(transform.DOScale(_finalScale, _fadeOutDuration))
                .SetEase(Ease.InExpo)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}