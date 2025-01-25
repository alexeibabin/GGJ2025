using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneLoader.MainMenu();
    }
}
