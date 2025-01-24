using System;
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
        startGame.onClick.AddListener(LoadMainGame);
        quitGame.onClick.AddListener(QuitGame);
        about.onClick.AddListener(LoadAbout);

        // Ensure the pause menu is initially inactive
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(false);
        }
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

    // Load the Main Menu Scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    // Load the Main Game Scene
    public void LoadMainGame()
    {
        SceneManager.LoadScene("MainGameScene");
        Debug.Log("Loading Main Game right now?");
    }

    // Load the Main Menu instead of quitting the application
    public void QuitGame()
    {
        Debug.Log("Returning to Main Menu...");
        LoadMainMenu(); // Calls the LoadMainMenu method
    }

    // Activate the Pause Menu
    private void PauseGame()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(true); // Activate the pause menu
            isPaused = true;
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("PauseMenuPrefab is not assigned!");
        }
    }

    // Deactivate the Pause Menu and resume the game
    private void ResumeGame()
    {
        if (pauseMenuPrefab != null)
        {
            pauseMenuPrefab.SetActive(false); // Deactivate the pause menu
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
        }
    }
}
