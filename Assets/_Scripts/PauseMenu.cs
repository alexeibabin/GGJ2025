using _Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public struct PauseMenuClosedEvent : IEvent
{
}

public class PauseMenu : MonoBehaviour
{
    [FormerlySerializedAs("restartButton")] [SerializeField] private Button retryButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button mainMenuButton;

    private bool isPaused = false;

    private void Awake()
    {
        isPaused = false;
        retryButton.onClick.AddListener(OnRetryClicked);
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

    private void OnRetryClicked()
    {
        Game.EventHub.Notify(new PauseMenuClosedEvent());
        Game.EventHub.Notify(new ResetEvent());
    }
}
