using _Scripts.Collectables;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class EndGameMenu : MonoBehaviour
    {
        private static readonly int GhostBlend = Shader.PropertyToID(GHOST_BLEND_MAT_PROPERTY);
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image convertedForestImage;
        [SerializeField] private float fillTime = 1f;

        private const string GHOST_BLEND_MAT_PROPERTY = "_GhostBlend";
        private float _fillAmount;

        private void Awake()
        {
            var coinsCount = PlayerPrefs.GetInt(ProjectConstants.SAVED_COINS, 0);
            SetupTextConfigurations(25);
            convertedForestImage.material.SetFloat(GhostBlend, 1);
        }

        private void SetupTextConfigurations(int coinsCount)
        {
            var seedsString = coinsCount > 1 ? "seeds" : "seed";
            coinsText.text = $"You managed to carry {coinsCount} {seedsString} of hope.";
        
            _fillAmount = Mathf.Min(1f, coinsCount / (float)ProjectConstants.COINS_GOAL);
        
            if (_fillAmount >= 1f)
            {
                messageText.text = "Enough seeds to restore the forest.";
            }
            else
            {
                messageText.text = "Not enough to fully restore the forest. Try again?";
            }
        }

        private void GoToMainMenu()
        {
            SceneLoader.MainMenu();
        }
    
        public void OnFadeFinished() // This method is called by the ScreenFader component
        {
            var targetFadeAmount = _fillAmount >= 1f ? 0f : 1f;
            var material = convertedForestImage.material;

            DOTween.Sequence()
                .AppendInterval(fillTime)
                .Append(DOTween.To(() => material.GetFloat(GhostBlend), x => material.SetFloat(GhostBlend, x), targetFadeAmount, fillTime))
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
    }
}
