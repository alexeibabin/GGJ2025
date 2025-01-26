using UnityEngine;

namespace _Scripts
{
    [DefaultExecutionOrder(-100)]
    public class Game : MonoBehaviour
    {
        private static Game Instance { get; set; }

        public static IEventHub EventHub { get; private set; }
        public static SessionData SessionData { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ResetGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ResetGame()
        {
            EventHub = new EventHub();
            SessionData = new SessionData();
            PlayerPrefs.DeleteKey(ProjectConstants.SAVED_COINS);
        }
    }
}