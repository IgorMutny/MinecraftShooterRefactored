using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterViewElements
{
    public class AudioController
    {
        private AudioSourceWrapper _idleSource;
        private AudioSourceWrapper _movingSource;
        private AudioSourceWrapper _attackingSource;
        private AudioSourceWrapper _damagedSource;
        private AudioSourceWrapper _diedSource;

        private CharacterAudioInfo _info;
        private Transform _transform;

        private bool _isAlive;
        private bool _isMoving;
        private float _speedMultiplier = 1.0f;

        private TimerWrapper _timer;
        private TimerSignal _idleSourceSignal;
        private TimerSignal _movingSourceSignal;

        private float _movingVolume = 0.25f;

        public AudioController(Transform transform, CharacterAudioInfo info)
        {
            _info = info;
            _transform = transform;

            CreateSources();
            CreateTimerSignals();

            _isAlive = true;
            _isMoving = false;
        }

        public void SetSpeed(float multiplier)
        {
            _speedMultiplier = multiplier;
        }

        public void StartMovement()
        {
            _isMoving = true;
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        public void Attack()
        {
            if (_attackingSource != null)
            {
                _attackingSource.Play();
            }
        }

        public void OnDamaged()
        {
            if (_damagedSource != null)
            {
                _damagedSource.Play();
            }
        }

        public void OnDied()
        {
            _isAlive = false;
            _isMoving = false;

            if (_diedSource != null)
            {
                _diedSource.Play();
            }
        }

        public void OnDestroy()
        {
            _idleSource = null;
            _movingSource = null;
            _attackingSource = null;
            _damagedSource = null;
            _diedSource = null;

            _timer.RemoveSignal(_idleSourceSignal);
            _timer.RemoveSignal(_movingSourceSignal);
        }

        private void CreateSources()
        {
            _idleSource = TryAddAudioSource(_info.IdleClips);
            _movingSource = TryAddAudioSource(_info.MovingClips);
            _attackingSource = TryAddAudioSource(_info.AttackingClips);
            _damagedSource = TryAddAudioSource(_info.DamagedClips);
            _diedSource = TryAddAudioSource(_info.DiedClips);

            if (_movingSource != null)
            {
                _movingSource.SetVolume(_movingVolume);
            }
        }

        private AudioSourceWrapper TryAddAudioSource(AudioClip[] clips)
        {
            if (clips.Length > 0)
            {
                AudioSourceWrapper result = new AudioSourceWrapper(_transform, true);
                result.SetClip(clips);
                return result;
            }
            else
            {
                return null;
            }
        }

        private void CreateTimerSignals()
        {
            _timer = ServiceLocator.Get<TimerWrapper>();
            SetIdleSourceSignal();
            SetMovingSourceSignal();
        }

        private void SetIdleSourceSignal()
        {
            if (_idleSource != null)
            {
                float delay = Random.Range(
                    _info.MinIdleClipRepeatingDelay, _info.MaxIdleClipRepeatingDelay);

                _idleSourceSignal = _timer.AddSignal(delay, TryPlayIdleClip);
            }
        }

        private void TryPlayIdleClip()
        {
            if (_isAlive == true)
            {
                if (_idleSource != null)
                {
                    _idleSource.Play();
                    SetIdleSourceSignal();
                }
            }
        }

        private void SetMovingSourceSignal()
        {
            if (_movingSource != null)
            {
                float delay = Random.Range(
                    _info.MinMovingClipRepeatingDelay, _info.MaxMovingClipRepeatingDelay);

                delay /= _speedMultiplier;
                _idleSourceSignal = _timer.AddSignal(delay, TryPlayMovingClip);
            }
        }

        private void TryPlayMovingClip()
        {
            if (_isAlive == true)
            {
                SetMovingSourceSignal();

                if (_isMoving == true)
                {
                    if (_movingSource != null)
                    {
                        _movingSource.Play();
                    }
                }
            }
        }
    }
}