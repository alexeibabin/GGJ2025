public static class Game
{
   public static IEventHub EventHub { get; private set; } = new EventHub();

   public static SessionData SessionData { get; private set; } = new SessionData();
}
