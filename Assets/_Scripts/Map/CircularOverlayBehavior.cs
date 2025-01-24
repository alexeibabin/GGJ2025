using System;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Map
{
    [RequireComponent(typeof(CircleSizeManager))]
    [RequireComponent(typeof(CircleCollisionHandler))]
    public class CircularOverlayBehavior : MonoBehaviour
    {
        [Range(CircleSizeManager.MIN_SIZE_VALUE, CircleSizeManager.MAX_SIZE_VALUE)]
        public float sizeValue;

        public event Action OnPlayerTouchEdge;

        private CircleSizeManager _circleSizeManager;
        private CircleCollisionHandler _collisionHandler;

        private Camera MainCamera => CameraUtil.GetMainCamera();

        private void Start()
        {
            _circleSizeManager = GetComponent<CircleSizeManager>();
            _collisionHandler = GetComponent<CircleCollisionHandler>();

            _collisionHandler.OnPlayerTouchEdge += HandlePlayerTouchEdge;

            _circleSizeManager.Initialize(sizeValue);
            CenterOnScreen(); 
        }

        private void Update()
        {
            _circleSizeManager.OnSizeValueChanged(sizeValue);
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
    }
}