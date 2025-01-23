using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Loads the specified scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quits the game and returns to the main menu
    public void QuitToMainMenu(string mainMenuSceneName)
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
