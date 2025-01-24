using UnityEngine;

namespace _Scripts.Utils
{
    public static class CameraUtil
    {
        private static Camera _cachedMainCamera;
        
        public static Camera GetMainCamera()
        {
            if (_cachedMainCamera != null && _cachedMainCamera.isActiveAndEnabled)
            {
                return _cachedMainCamera;
            }
            
            _cachedMainCamera = Camera.main;
            
            if (_cachedMainCamera == null)
            {
                JamLogger.LogWarning("No Main Camera found in the scene! Make sure your camera has the 'MainCamera' tag.");
            }

            return _cachedMainCamera;
        }
        
        public static void ClearCachedCamera()
        {
            _cachedMainCamera = null;
        }
    }

}