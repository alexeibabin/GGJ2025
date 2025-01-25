using System;
using UnityEngine;

namespace _Scripts.Map
{
    public class EnvironmentManager : MonoBehaviour
    {
        private IDisposable _transitionStartedSubscription;
        private Animator _animator;
        
        private static readonly int Level = Animator.StringToHash(ProjectConstants.LEVEL_ANIM_KEY);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _transitionStartedSubscription = Game.EventHub.Subscribe<TransitionStartedEvent>(PlayTransitionAnimation);
        }

        private void OnDisable()
        {
            _transitionStartedSubscription.Dispose();
        }

        private void PlayTransitionAnimation(TransitionStartedEvent transitionStartedEvent)
        {
            _animator.SetInteger(Level, transitionStartedEvent.transitionNumber);
        }
    }
}

