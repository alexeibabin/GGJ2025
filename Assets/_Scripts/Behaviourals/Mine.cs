using UnityEngine;
using DG.Tweening;

namespace _Scripts.Behaviourals
{
    public class Mine : MonoBehaviour
    {
        private Sequence _spinSequence;
        [SerializeField] private float minInterval = 2f;
        [SerializeField] private float maxInterval = 10f;
        [SerializeField] private float pushForce = 0.1f;
        [SerializeField] private float maxDistanceFromCenter = 5f; 

        private void Start()
        {
            ScheduleNextSpin();
        }

        private void Update()
        {
            CheckDistanceFromCenter();
        }

        private void ScheduleNextSpin()
        {
            var interval = Random.Range(minInterval, maxInterval);
            Invoke(nameof(StartSpinning), interval);
        }

        private void StartSpinning()
        {
            _spinSequence = DOTween.Sequence();
            _spinSequence.Append(transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .OnComplete(ScheduleNextSpin);
        }

        private void CheckDistanceFromCenter()
        {
            if (Vector3.Distance(transform.position, Vector3.zero) > maxDistanceFromCenter)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(ProjectConstants.PROJECTILE_TAG))
            {
                Vector2 collisionNormal = collision.contacts[0].normal;
                GetComponent<Rigidbody2D>().AddForce(-collisionNormal * pushForce, ForceMode2D.Impulse);
            }
        }

        private void OnDestroy()
        {
            _spinSequence?.Kill();
        }
    }
}