using _Scripts;
using _Scripts.Collectables;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image convertedForestImage;
    [SerializeField] private float fillTime = 1f;

    private void Awake()
    {
        int coinsCount = Game.SessionData.GetCollectableCount(ECollectableType.Coin);
        PlayConversionScreen(coinsCount);
    }

    private void PlayConversionScreen(int coinsCount)
    {
        string seedsString = coinsCount > 1 ? "seeds" : "seed";
        coinsText.text = $"You managed to carry {coinsCount} {seedsString} of hope.";
        
        float fillAmount = Mathf.Min(1f, coinsCount / (float)ProjectConstants.COINS_GOAL);
        if (fillAmount >= 1f)
        {
            messageText.text = "Enough seeds to restore the forest.";
        }
        else
        {
            messageText.text = "Not enough to fully restore the forest. Try again?";
        }

        DOTween.Sequence()
            .AppendInterval(fillTime)
            .Append(convertedForestImage.DOFade(fillAmount, fillTime))
            .AppendInterval(fillTime)
            .Append(coinsText.DOFade(1, fillTime))
            .AppendInterval(fillTime)
            .Append(messageText.DOFade(1, fillTime))
            .AppendInterval(fillTime)
            .OnComplete(() =>
            {
                mainMenuButton.gameObject.SetActive(true);
                mainMenuButton.onClick.AddListener(GoToMainMenu);
            });
    }

    private void GoToMainMenu()
    {
        SceneLoader.MainMenu();
    }
}
