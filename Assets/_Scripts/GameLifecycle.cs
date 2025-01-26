using System.Collections;
using _Scripts;
using _Scripts.Collectables;
using _Scripts.Spawner;
using _Scripts.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public struct GameTimerStartEvent : IEvent
{
}

public struct TransitionStartedEvent : IEvent
{
    public int transitionNumber;
    
    public TransitionStartedEvent(int transition)
    {
        transitionNumber = transition;
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
        Time.timeScale = 1;
        Game.SessionData.IsPaused = false;
        
        Game.EventHub.Subscribe<PlayerDeathEvent>(PlayerDeath);
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
        StartCoroutine(ResetDataDelayed());
        
        Time.timeScale = 1;
        Game.SessionData.IsPaused = false;

        if (lifecycleCoro != null)
        {
            StopCoroutine(lifecycleCoro);
            lifecycleCoro = null;
        }
    }
    
    private void PlayerDeath(PlayerDeathEvent evt)
    {
    }
    
    IEnumerator ResetDataDelayed()
    {
        yield return null; 
        Game.SessionData.ResetSessionData();
    }

    private void Pause(PauseEvent evt)
    {
        Game.SessionData.IsPaused = !Game.SessionData.IsPaused;
        Time.timeScale = Game.SessionData.IsPaused ? 0 : 1;

        if (!Game.SessionData.IsPaused && lifecycleCoro == null)
        {
            lifecycleCoro = StartCoroutine(TimerLoop());
        }
        else if (Game.SessionData.IsPaused && lifecycleCoro != null)
        {
            StopCoroutine(lifecycleCoro);
            lifecycleCoro = null;
        }
    }

    private IEnumerator TimerLoop()
    {
        while (true)
        {
            if (!Game.SessionData.IsPaused)
            {
                Game.SessionData.ProgressTimer += Time.deltaTime;
                Game.SessionData.TimeSinceLastTransition += Time.deltaTime;

                if (Game.SessionData.TimeSinceLastTransition >= timeBetweenTransitions)
                {
                    Game.SessionData.TransitionsCompleted++;
                    Game.SessionData.TimeSinceLastTransition = 0;

                    JamLogger.LogInfo($"Transition #{Game.SessionData.TransitionsCompleted} has started");
                    Game.EventHub.Notify(new TransitionStartedEvent(Game.SessionData.TransitionsCompleted));
                }

                if (Mathf.RoundToInt(Game.SessionData.TimeSinceLastTransition) >
                    Mathf.RoundToInt(Game.SessionData.TimeSinceLastTransition - Time.deltaTime))
                {
                    Game.EventHub.Notify(new AttemptSpawnEvent());
                }
            }

            yield return null;
        }
    }

    private void StartTimer(GameTimerStartEvent evt)
    {
        JamLogger.LogInfo($"Game timer has started");
        lifecycleCoro = StartCoroutine(TimerLoop());
    }
    
    [ButtonGroup] 
    private void StartGame()
    {
        Game.EventHub.Notify(new GameTimerStartEvent());
    }
}
