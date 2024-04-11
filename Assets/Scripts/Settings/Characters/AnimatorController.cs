using UnityEngine;

namespace CharacterViewElements
{
    public class AnimatorController
    {
        private Animator _animator;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void ChangeSpeed(float multiplier)
        {
            if (_animator != null)
            {
                _animator.speed = multiplier;
            }
        }

        public void StartMovement()
        {
            if (_animator != null)
            {
                _animator.SetBool("isMoving", true);
            }
        }

        public void StopMovement()
        {
            if (_animator != null)
            {
                _animator.SetBool("isMoving", false);
            }
        }

        public void Attack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("isAttacking");
            }
        }

        public void OnDied()
        {
            if (_animator != null)
            {
                _animator.SetBool("isMoving", false);
            }
        }
    }
}