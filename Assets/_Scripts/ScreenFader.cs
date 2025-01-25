using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeTime;
    
    private void FadeIn()
    {
        _image.CrossFadeAlpha(1, _fadeTime, true);
    }
    
    private void FadeOut()
    {
        _image.CrossFadeAlpha(0, _fadeTime, true);
    }
}
