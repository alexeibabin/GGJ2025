using System;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Map
{
    public class CircleCollisionHandler : MonoBehaviour
    {
        public event Action OnPlayerTouchEdge;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(ProjectConstants.PLAYER_TAG))
            {
                OnPlayerTouchEdge?.Invoke();
                JamLogger.LogInfo("Player touched the edge!");
            }
        }
    }
}