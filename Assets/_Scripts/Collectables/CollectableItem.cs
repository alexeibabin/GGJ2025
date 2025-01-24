using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Collectables
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectableItem : MonoBehaviour, ICollectable
    {
        [SerializeField] private ECollectableType collectableType;

        public void Collect()
        {
            Game.SessionData.AddCollectable(collectableType);

            JamLogger.LogInfo($"Collectable of type {collectableType} collected. Total: {Game.SessionData.GetCollectableCount(collectableType)}");

            Destroy(gameObject); // TODO: Object pooling?
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ProjectConstants.PLAYER_TAG))
            {
                Collect();
            }
        }
    }
}