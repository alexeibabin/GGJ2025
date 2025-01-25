using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouLoseCanvasController : MonoBehaviour
{
    public GameObject screen;
    public float timeToLoseScreen = 0.5f;
    private bool isShowingLoseScreen = false;

    private void Awake()
    {
        Game.EventHub.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        isShowingLoseScreen = true;
        StartCoroutine(ShowLoseScreenAfterDelay());
    }

    private IEnumerator ShowLoseScreenAfterDelay()
    {
        yield return new WaitForSeconds(timeToLoseScreen);
        screen.SetActive(true);
        Game.EventHub.Notify(new PauseEvent());
    }

    public void Retry()
    {
        isShowingLoseScreen = false;
        Game.EventHub.Notify(new ResetEvent());
        screen.SetActive(false);
    }

    public void MainMenu()
    {
        SceneLoader.MainMenu();
    }
}
