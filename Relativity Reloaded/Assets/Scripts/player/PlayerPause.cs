namespace Project.Internal.Scripts.Enemies.player
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerPause
    {
        private readonly List<Animator> _animators = new List<Animator>();
        private readonly List<Rigidbody> _rigidbodies = new List<Rigidbody>();
        public bool IsGamePaused { get; private set; }

        public PlayerPause()
        {
            FindAllAnimatorsAndRigidbodies();
        }

        public void TogglePauseGame()
        {
            IsGamePaused = !IsGamePaused;

            if (IsGamePaused)
            {
                foreach (var animator in _animators)
                {
                    animator.enabled = false;
                }
                foreach (var rb in _rigidbodies)
                {
                    rb.isKinematic = true;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                foreach (var animator in _animators)
                {
                    animator.enabled = true;
                }
                foreach (var rb in _rigidbodies)
                {
                    rb.isKinematic = false;
                }
            }
        }

        private void FindAllAnimatorsAndRigidbodies()
        {
            _animators.AddRange(Object.FindObjectsOfType<Animator>());
            _rigidbodies.AddRange(Object.FindObjectsOfType<Rigidbody>());
        }
    }

}