using System.Collections;
using UnityEngine;

public struct GameTimerStartEvent : IEvent
{
}

public struct TransitionStartedEvent : IEvent
{
}

public struct PauseEvent : IEvent
{
}

public struct ResetEvent : IEvent
{
}

public class GameLifecycle : MonoBehaviour
{
    [SerializeField] private float timeBetweenTransitions;
    [SerializeField] private float countInterval;

    private Coroutine lifecycleCoro;
    
    private void Awake()
    {
        Game.EventHub.Subscribe<GameTimerStartEvent>(StartTimer);
        Game.EventHub.Subscribe<PauseEvent>(Pause);
        Game.EventHub.Subscribe<ResetEvent>(Reset);

    }

    private void Reset(ResetEvent evt)
    {
        Game.SessionData.ProgressTimer = 0;
        Game.SessionData.TimeSinceLastTransition = 0;

        if (lifecycleCoro != null)
        {
            StopCoroutine(lifecycleCoro);
        }
    }

    private void Pause(PauseEvent evt)
    {
        Game.SessionData.isPaused = !Game.SessionData.isPaused;
    }

    private void StartTimer(GameTimerStartEvent evt)
    {
        lifecycleCoro = StartCoroutine(TimerLoop());
    }

    private IEnumerator TimerLoop()
    {
        if (Game.SessionData.isPaused)
        {
            Game.SessionData.ProgressTimer += Time.deltaTime;
            Game.SessionData.TimeSinceLastTransition += Time.deltaTime;

            if (Game.SessionData.TimeSinceLastTransition >= timeBetweenTransitions)
            {
                Game.SessionData.TimeSinceLastTransition = 0;
                Game.EventHub.Notify(new TransitionStartedEvent());
            }
            
            yield return new WaitForSeconds(countInterval);
        }
    }
}
