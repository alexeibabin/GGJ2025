using System;
using System.Collections;
using UniRx;
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
        Game.SessionData.ProgressTimer = 0;
        Game.SessionData.TimeSinceLastTransition = 0;
        Game.SessionData.ClearCollectables();

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
        lifecycleCoro = StartCoroutine(TimerLoop());
    }

    private IEnumerator TimerLoop()
    {
        if (!Game.SessionData.IsPaused)
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
