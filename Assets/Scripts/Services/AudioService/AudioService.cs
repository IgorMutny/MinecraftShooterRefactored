using UnityEngine;
using Object = UnityEngine.Object;

public class AudioService : IService
{
    private UIAudioSource _audioSource;

    public AudioListener Listener { get; private set; }

    public float Volume { get; private set; }

    public AudioService(MiscObjectsCollection uiCollection)
    {
        Volume = ServiceLocator.Get<GameDataService>().SoundVolume;

        GameObject audioSourceObject = GameObject.Instantiate(uiCollection.UIAudioSource);
        Object.DontDestroyOnLoad(audioSourceObject);
        _audioSource = audioSourceObject.GetComponent<UIAudioSource>();
        _audioSource.Initialize(this);
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
    }

    public void OnButtonClicked()
    {
        _audioSource.OnButtonClicked();
    }

    public void Play(AudioClip clip)
    {
        _audioSource.Play(clip);
    }

    public void FindListener()
    {
        Listener = GameObject.FindObjectOfType<AudioListener>();
    }
}
