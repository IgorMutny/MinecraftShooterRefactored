using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class AudioService : IService
{
    private UIAudioSource _audioSource;
    private float _volume;

    public event Action<float> VolumeChanged;

    public float Volume => _volume;

    public AudioService(MiscObjectsCollection uiCollection)
    {
        _volume = ServiceLocator.Get<GameDataService>().SoundVolume;

        GameObject audioSourceObject = GameObject.Instantiate(uiCollection.UIAudioSource);
        Object.DontDestroyOnLoad(audioSourceObject);
        _audioSource = audioSourceObject.GetComponent<UIAudioSource>();
        _audioSource.Initialize(this);
    }

    public void ChangeSoundVolume(float volume)
    {
        _volume = volume;
        VolumeChanged?.Invoke(volume);
    }

    public void OnButtonClicked()
    {
        _audioSource.OnButtonClicked();
    }
}
