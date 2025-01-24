using System.Collections;
using _Scripts.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public struct GameTimerStartEvent : IEvent
{
}

public struct TransitionStartedEvent : IEvent
{
    public float progressTimer;
    
    public TransitionStartedEvent(float progressionTimer)
    {
        progressTimer = progressionTimer;
    }
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
    
    private void Start()
    {
        Game.EventHub.Subscribe<GameTimerStartEvent>(StartTimer);
        Game.EventHub.Subscribe<PauseEvent>(Pause);
        Game.EventHub.Subscribe<ResetEvent>(Reset);
        Game.SessionData.CollectedItems.ObserveCountChanged().Subscribe(StartGameOnFirstCollect).AddTo(this);
    }

    private void StartGameOnFirstCollect(int amount)
    {
        if (amount == 1 && lifecycleCoro == null)
        {
            Game.EventHub.Notify(new GameTimerStartEvent());
        }
    }

    private void Reset(ResetEvent evt)
    {
        Game.SessionData.ResetSessionData();
        
        if (lifecycleCoro != null)
        {
            StopCoroutine(lifecycleCoro);
            lifecycleCoro = null;
        }
    }

    private void Pause(PauseEvent evt)
    {
        Game.SessionData.IsPaused = !Game.SessionData.IsPaused;
    }

    private void StartTimer(GameTimerStartEvent evt)
    {
        JamLogger.LogInfo($"Game timer has started");
        lifecycleCoro = StartCoroutine(TimerLoop());
    }

    private IEnumerator TimerLoop()
    {
        while (!Game.SessionData.IsPaused)
        {
            Game.SessionData.ProgressTimer += Time.deltaTime;
            Game.SessionData.TimeSinceLastTransition += Time.deltaTime;

            if (Game.SessionData.TimeSinceLastTransition >= timeBetweenTransitions)
            {
                //  Move this to transition animation complete?
                Game.SessionData.TransitionsCompleted++;
                Game.SessionData.TimeSinceLastTransition = 0;
                
                JamLogger.LogInfo($"Transition #{Game.SessionData.TransitionsCompleted} has started");
                Game.EventHub.Notify(new TransitionStartedEvent(Game.SessionData.ProgressTimer));
            }

            yield return null;
        }
    }
    
    [ButtonGroup] 
    private void StartGame()
    {
        Game.EventHub.Notify(new GameTimerStartEvent());
    }
}
