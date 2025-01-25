using UnityEngine;

namespace _Scripts.Utils
{
    public static class ComponentHelper
    {
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            
            return component != null;
        }
    }
}