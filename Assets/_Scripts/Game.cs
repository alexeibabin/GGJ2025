using UnityEngine;

public class Game : MonoBehaviour
{
   public static IEventHub EventHub { get; private set; }

   public static SessionData SessionData { get; private set; }

   private void Awake()
   {
      EventHub = new EventHub();
      DontDestroyOnLoad(gameObject);
      Clear();
   }

   public static void Clear()
   {
      SessionData = new SessionData();
   }
}
