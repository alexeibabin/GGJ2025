using System;
using UnityEngine;

public class Game : MonoBehaviour
{
   public static IEventHub EventHub { get; private set; }

   public static SessionData SessionData { get; private set; } = new SessionData();

   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
   }

   public static void Clear()
   {
      SessionData = new SessionData();
   }
}
