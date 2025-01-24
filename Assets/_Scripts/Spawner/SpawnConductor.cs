using System;
using System.Globalization;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Spawner
{
    public class SpawnConductor : MonoBehaviour
    {
        private void Start()
        {
            Game.EventHub.Subscribe<TransitionStartedEvent>(OnTransitionStarted);
        }

        private void OnTransitionStarted(TransitionStartedEvent evt)
        {
            JamLogger.LogInfo(evt.progressTimer.ToString(CultureInfo.InvariantCulture));
        }
    }
}