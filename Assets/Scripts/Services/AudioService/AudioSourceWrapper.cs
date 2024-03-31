using UnityEngine;

public class AudioSourceWrapper
{
    private AudioService _service;
    private AudioSource _audioSource;

    public AudioSourceWrapper(GameObject gameObject)
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _service = ServiceLocator.Get<AudioService>();
        _audioSource.volume = _service.Volume;
        _service.VolumeChanged += OnVolumeChanged;
    }

    public AudioSourceWrapper(GameObject gameObject, AudioService service)
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _service = service;
        _audioSource.volume = _service.Volume;
        _service.VolumeChanged += OnVolumeChanged;
    }

    public void Destroy()
    {
        _service.VolumeChanged -= OnVolumeChanged;
        GameObject.Destroy(_audioSource);
    }

    public void Play()
    {
        _audioSource.Play();
    }

    public void SetClip(AudioClip clip)
    { 
        _audioSource.clip = clip;
    }

    public void IgnoreListenerPause(bool value)
    {
        _audioSource.ignoreListenerPause = value;
    }

    private void OnVolumeChanged(float volume)
    {
        _audioSource.volume = volume;
    }
}
