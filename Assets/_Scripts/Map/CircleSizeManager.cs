using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Map
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CircleSizeManager : MonoBehaviour
    {
        public const int MIN_SIZE_VALUE = 4;
        public const int MAX_SIZE_VALUE = 10;
        private const float INNER_CIRCLE_OFFSET = 0.1f;

        private Transform _innerCircle;
        private Transform _circleTransform;
        private CircleCollider2D _outerCircleCollider;
        private CircleCollider2D _innerCircleCollider;
        private SpriteRenderer _outerSpriteRenderer;

        public void Initialize(float initialSize)
        {
            CacheRequiredComponents();
            OnSizeValueChanged(initialSize);
        }

        private void CacheRequiredComponents()
        {
            _circleTransform = GetComponent<Transform>();
            _outerCircleCollider = GetComponent<CircleCollider2D>();
            _outerSpriteRenderer = GetComponent<SpriteRenderer>();
            _innerCircle = transform.GetChild(0);

            if (_innerCircle != null)
            {
                _innerCircleCollider = _innerCircle.GetComponent<CircleCollider2D>();
            }
            else
            {
                JamLogger.LogWarning("Inner circle not found!");
            }
        }

        public void OnSizeValueChanged(float sizeValue)
        {
            var newSize = Mathf.Clamp(sizeValue, MIN_SIZE_VALUE, MAX_SIZE_VALUE);
            _circleTransform.localScale = new Vector3(newSize, newSize, 1);

            UpdateOuterCircleSize();
            UpdateInnerCircleSize();
        }

        private void UpdateOuterCircleSize()
        {
            if (_outerCircleCollider != null && _outerSpriteRenderer != null)
            {
                var diameterWorldUnits = _outerSpriteRenderer.bounds.size.x;

                _outerCircleCollider.radius = diameterWorldUnits / 2f / _circleTransform.lossyScale.x;
            }
        }

        private void UpdateInnerCircleSize()
        {
            if (_innerCircle != null && _innerCircleCollider != null)
            {
                var outerRadius = _outerCircleCollider.radius * _circleTransform.lossyScale.x;
                var innerRadius = Mathf.Max(outerRadius - INNER_CIRCLE_OFFSET, MIN_SIZE_VALUE / 2f);

                _innerCircle.localScale = new Vector3(innerRadius * 2f, innerRadius * 2f, 1);
                _innerCircleCollider.radius = innerRadius / _innerCircle.lossyScale.x;
            }
        }
    }
}
