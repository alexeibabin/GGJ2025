using System;
using UnityEngine;

namespace _Scripts.Map
{
    public class EnvironmentManager : MonoBehaviour
    {
        private IDisposable _transitionStartedSubscription;
        private IDisposable _resetSubscription;
        private Animator _animator;
        
        private static readonly int Level = Animator.StringToHash(ProjectConstants.LEVEL_ANIM_KEY);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _resetSubscription = Game.EventHub.Subscribe<ResetEvent>(ResetEnvironment);
            _transitionStartedSubscription = Game.EventHub.Subscribe<TransitionStartedEvent>(PlayTransitionAnimation);
        }

        private void OnDisable()
        {
            _transitionStartedSubscription.Dispose();
            _resetSubscription.Dispose();
        }

        private void PlayTransitionAnimation(TransitionStartedEvent transitionStartedEvent)
        {
            _animator.SetInteger(Level, transitionStartedEvent.transitionNumber);
        }
        
        private void ResetEnvironment(ResetEvent evt)
        {
            _animator.SetInteger(Level, 0);
        }
    }
}

