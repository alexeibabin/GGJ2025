using _Scripts.Collectables;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Text text;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        int coinsCount = Game.SessionData.GetCollectableCount(ECollectableType.Coin);
        
        //text.text = $"You managed to carry {coinsCount} seeds of hope.";
    }

    private void GoToMainMenu()
    {
        SceneLoader.MainMenu();
    }
}
