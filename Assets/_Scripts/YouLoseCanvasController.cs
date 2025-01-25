using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouLoseCanvasController : MonoBehaviour
{
    public GameObject screen;
    public float timeToLoseScreen = 0.5f;
    private bool isShowingLoseScreen = false;

    private void OnDisable()
    {
        Game.SessionData.BubbleHealth.onValueChanged -= BubbleHealthOnonValueChanged;
    }

    private void OnEnable()
    {
        Game.SessionData.BubbleHealth.onValueChanged += BubbleHealthOnonValueChanged;
    }

    private void BubbleHealthOnonValueChanged(float obj)
    {
        if (Game.SessionData.BubbleHealth.value <= 0 && !isShowingLoseScreen)
        {
            isShowingLoseScreen = true;
            StartCoroutine(ShowLoseScreenAfterDelay());
        }
    }

    private IEnumerator ShowLoseScreenAfterDelay()
    {
        yield return new WaitForSeconds(timeToLoseScreen);
        screen.SetActive(true);
    }

    public void Retry()
    {
        SceneLoader.MainGame();
    }

    public void MainMenu()
    {
        SceneLoader.MainMenu();
    }
}
