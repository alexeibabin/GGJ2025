using System;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    public BubbleMovement MovementScript;
    public PlayerDeath DeathScript;
    private void Awake()
    {
        Game.EventHub.Subscribe<ResetEvent>(OnGameReset);
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
        MovementScript.enabled = false;
        DeathScript.PlayDeathSequence();
    }
    
    private void OnGameReset(ResetEvent evt)
    {
        MovementScript.enabled = true;
        MovementScript.ResetMovement();    
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyScript = other.gameObject.GetComponent<Enemy>();
            Game.SessionData.BubbleHealth.value -= enemyScript.damage;
        }
    }
}
