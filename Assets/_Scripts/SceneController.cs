using System;
using _Scripts;
using _Scripts.Spawner;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button startGame;
    [SerializeField] private Button quitGame;
    [SerializeField] private Button about;
    [SerializeField] private GameObject pauseMenuPrefab; // Reference to the Pause Menu Prefab

    private bool isPaused = false;

    private void Awake()
    {
        if(startGame != null)
            startGame.onClick.AddListener(LoadMainGame);
        
        if (quitGame != null)
            quitGame.onClick.AddListener(QuitGame);
        
        if (about != null)  
            about.onClick.AddListener(LoadAbout);

        Game.EventHub.Subscribe<PauseMenuClosedEvent>(OnPauseMenuClosed);
        Game.EventHub.Subscribe<FadeFinishedEvent>(evt => LoadEndGame());

        // Ensure the pause menu is initially inactive
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(false);
        }
    }

    private void LoadEndGame()
    {
        SceneManager.LoadScene(ProjectConstants.END_GAME_SCENE_NAME);
    }

    private void OnPauseMenuClosed(PauseMenuClosedEvent evt)
    {
        ResumeGame();
    }

    private void Update()
    {
        // Check for the Esc key press to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void LoadAbout()
    {
        throw new NotImplementedException();
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene(ProjectConstants.MAIN_MENU_SCENE_NAME);
    }

    public static void MainGame()
    {
        SceneManager.LoadScene(ProjectConstants.MAIN_GAME_SCENE_NAME);
        JamLogger.LogInfo("Loading Main Game right now?");
    }

    //im sorry for this name
    public static void QuitGameStatic()
    {
        JamLogger.LogInfo("Returning to Main Menu...");
        MainMenu(); // Calls the LoadMainMenu method
    }
    // Load the Main Menu Scene
    public void LoadMainMenu()
    {
        MainMenu();
    }

    // Load the Main Game Scene
    public void LoadMainGame()
    {
        MainGame();
    }

    // Load the Main Menu instead of quitting the application
    public void QuitGame()
    {
        QuitGameStatic();
    }

    // Activate the Pause Menu
    private void PauseGame()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(true); // Activate the pause menu
            isPaused = true;
            Game.EventHub.Notify(new PauseEvent());// Pause the game
        }
        else
        {
            JamLogger.LogError("PauseMenuPrefab is not assigned!");
        }
    }

    // Deactivate the Pause Menu and resume the game
    private void ResumeGame()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(false); // Deactivate the pause menu
            isPaused = false;
            Game.EventHub.Notify(new PauseEvent());// Pause the game
        }
    }
}
