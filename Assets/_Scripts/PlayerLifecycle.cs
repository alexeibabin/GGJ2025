using System;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    private void Awake()
    {
        //Game.EventHub.Subscribe<ResetEvent>(Reset);
        Game.SessionData.BubbleHealth.onValueChanged += CheckDeathScenario;
    }

    private void CheckDeathScenario(float health)
    {
        if(health > 0)
            return;

        PlayDeathSequence();
    }

    private void PlayDeathSequence()
    {
        throw new NotImplementedException();
    }
}
