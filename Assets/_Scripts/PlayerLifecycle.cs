using System;
using UnityEngine;

public struct PlayerDeathEvent : IEvent
{
}

public class PlayerLifecycle : MonoBehaviour
{
    public BubbleMovement MovementScript;
    
    [SerializeField] private GameObject livingArt;
    [SerializeField] private GameObject deathArt;
    
    private void Awake()
    {
        Game.EventHub.Subscribe<ResetEvent>(OnGameReset);
        Game.SessionData.BubbleHealth.onValueChanged += CheckDeathScenario;
    }

    private void CheckDeathScenario(float health)
    {
        if(health > 0)
            return;

        Game.EventHub.Notify(new PlayerDeathEvent());
        PlayDeathSequence();
    }

    private void PlayDeathSequence()
    {
        livingArt.SetActive(false);
        deathArt.SetActive(true);
    }
    
    private void OnGameReset(ResetEvent evt)
    {
        livingArt.SetActive(true);
        deathArt.SetActive(false);
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
