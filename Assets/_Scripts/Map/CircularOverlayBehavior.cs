using System;
using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Map
{
    [RequireComponent(typeof(CircleSizeManager))]
    [RequireComponent(typeof(CircleCollisionHandler))]
    public class CircularOverlayBehavior : MonoBehaviour
    {
        [Range(CircleSizeManager.MIN_SIZE_VALUE, CircleSizeManager.MAX_SIZE_VALUE)]
        [SerializeField] private float sizeValue;
        [SerializeField] private float tweenDuration = 0.5f;
        [SerializeField] private Ease tweenEase = Ease.OutElastic;

        public event Action OnPlayerTouchEdge;

        private CircleSizeManager _circleSizeManager;
        private CircleCollisionHandler _collisionHandler;
        private float _currentSize;
        
        private Camera MainCamera => CameraUtil.GetMainCamera();

        private void Start()
        {
            _circleSizeManager = GetComponent<CircleSizeManager>();
            _collisionHandler = GetComponent<CircleCollisionHandler>();

            _collisionHandler.OnPlayerTouchEdge += HandlePlayerTouchEdge;

            _currentSize = sizeValue;
            _circleSizeManager.Initialize(_currentSize);
            CenterOnScreen();
        }

        private void Update()
        {
            if (Math.Abs(_currentSize - sizeValue) > 0.01f)
            {
                TweenSizeValue(sizeValue);
            }
        }

        private void LateUpdate()
        {
            CenterOnScreen();
        }

        private void HandlePlayerTouchEdge()
        {
            OnPlayerTouchEdge?.Invoke();
        }

        private void CenterOnScreen()
        {
            if (MainCamera != null)
            {
                var screenCenterPosition = MainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, MainCamera.nearClipPlane));
                transform.position = new Vector3(screenCenterPosition.x, screenCenterPosition.y, transform.position.z);
            }
        }

        private void TweenSizeValue(float targetSize)
        {
            DOTween.To(() => _currentSize, x => _currentSize = x, targetSize, tweenDuration)
                .SetEase(tweenEase)
                .OnUpdate(() =>
                {
                    _circleSizeManager.OnSizeValueChanged(_currentSize);
                });
        }
    }
}
