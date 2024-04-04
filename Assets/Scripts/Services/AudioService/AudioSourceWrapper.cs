using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioSourceWrapper
{
    private AudioService _service;
    private Transform _transform;
    private AudioSource _audioSource;
    private AudioClip[] _clips;
    private bool _ignoreListenerPause;
    private bool _is3D;
    private float _minDistance = 1;
    private float _maxDistance = 64;
    private float _innerVolume = 1;

    private AudioListener _listener => _service.Listener;
    private float Volume => _service.Volume;

    public AudioSourceWrapper(Transform transform, bool is3D = false)
    {
        _service = ServiceLocator.Get<AudioService>();
        SetAudioSource(transform, is3D);
    }

    public AudioSourceWrapper(Transform transform, AudioService service, bool is3D = false)
    {
        _service = service;
        SetAudioSource(transform, is3D);
    }

    private void SetAudioSource(Transform transform, bool is3D)
    {
        _transform = transform;
        _audioSource = transform.AddComponent<AudioSource>();
        _is3D = is3D;
    }

    public void SetClip(AudioClip clip)
    {
        _clips = new AudioClip[] {clip };
    }

    public void SetClip(AudioClip[] clips)
    {
        _clips = new AudioClip[clips.Length];
        Array.Copy(clips, _clips, clips.Length);
    }

    public void SetSpeed(float multiplier)
    {
        _audioSource.pitch = multiplier;
    }

    public void SetVolume(float innerVolume)
    {
        _innerVolume = innerVolume;
    }

    public void Play()
    {
        if (_ignoreListenerPause == false && AudioListener.pause == true)
        {
            return;
        }

        if (_listener == null)
        {
            _service.FindListener();
        }

        int rnd = Random.Range(0, _clips.Length);
        _audioSource.clip = _clips[rnd];
        _audioSource.volume = GetVolumeByDistance();

        _audioSource.Play();
    }

    public void IgnoreListenerPause(bool value)
    {
        _ignoreListenerPause = value;
    }

    private float GetVolumeByDistance()
    {
        if (_is3D == false)
        {
            return Volume * _innerVolume;
        }
        else
        {
            Vector3 listenerPosition = _listener.transform.position;
            Vector3 sourcePosition = _transform.position;

            float distance = Vector3.Distance(listenerPosition, sourcePosition);
            if (distance < _minDistance)
            {
                distance = _minDistance;
            }

            float k = (_maxDistance - distance) / (_maxDistance - _minDistance);
            float volume = Volume * k * _innerVolume;
            volume = Mathf.Clamp01(volume);

            return volume;
        }
    }
}
