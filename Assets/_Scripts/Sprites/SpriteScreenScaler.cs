using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteScreenScaler : MonoBehaviour
    {
        private SpriteRenderer _mySprite;
        private float SpriteWidth => _mySprite.sprite.bounds.size.x;
        private float SpriteHeight => _mySprite.sprite.bounds.size.y;

        private void Awake()
        {
            _mySprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ScaleSprite();
        }

        private void ScaleSprite()
        {
            var cam = CameraUtil.GetMainCamera();
        
            if (cam != null)
            {
                var screenHeightInUnits = cam.orthographicSize * 2;
                var screenWidthInUnits = screenHeightInUnits * cam.aspect;

                _mySprite.transform.localScale = new Vector3(screenWidthInUnits / SpriteWidth, screenHeightInUnits / SpriteHeight, 1);
                _mySprite.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, _mySprite.transform.position.z);
            }
            else
            {
                JamLogger.LogError("No main camera found");
            }
        }
    }
}