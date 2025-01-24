using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private Button startGame;
    [SerializeField] private Button quitGame;
    [SerializeField] private Button about;

    private void Awake()
    {
        if (startGame != null)
        {
            startGame.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("start Game button is not assigned!");
        }
        
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
}