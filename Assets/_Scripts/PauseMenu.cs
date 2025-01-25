using UnityEngine;
using UnityEngine.UI;

public struct PauseMenuClosedEvent : IEvent
{
}

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button mainMenuButton;

    private bool isPaused = false;

    private void Awake()
    {
        isPaused = false;
        restartButton.onClick.AddListener(OnRestartClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
        mainMenuButton.onClick.AddListener(OnReturnToMainMenuClicked);
    }

    private void OnReturnToMainMenuClicked()
    {
        SceneLoader.QuitGameStatic();
    }

    private void OnBackButtonClicked()
    {
        Game.EventHub.Notify(new PauseMenuClosedEvent());
    }

    private void OnRestartClicked()
    {
        Game.EventHub.Notify(new PauseMenuClosedEvent());
        Game.EventHub.Notify(new ResetEvent());
    }
}
