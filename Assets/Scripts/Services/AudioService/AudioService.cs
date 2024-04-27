using UnityEngine;
using Object = UnityEngine.Object;

public class AudioService : IService
{
    private UIAudioSource _audioSource;

    public AudioListener Listener { get; private set; }

    public float AudioVolume { get; private set; }
    public float MusicVolume { get; private set; }

    public AudioService(MiscObjectsCollection uiCollection)
    {
        AudioVolume = ServiceLocator.Get<GameDataService>().SoundVolume;

        GameObject audioSourceObject = GameObject.Instantiate(uiCollection.UIAudioSource);
        Object.DontDestroyOnLoad(audioSourceObject);
        _audioSource = audioSourceObject.GetComponent<UIAudioSource>();
        _audioSource.Initialize(this);
    }

    public void SetVolume(float audio, float music)
    {
        AudioVolume = audio;
        MusicVolume = music;

        _audioSource.UpdateMusicVolume();
    }

    public void OnButtonClicked()
    {
        _audioSource.OnButtonClicked();
    }

    public void Play(AudioClip clip)
    {
        _audioSource.Play(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _audioSource.PlayMusic(clip);
    }

    public void StopMusic()
    {
        _audioSource.StopMusic();
    }

    public void FindListener()
    {
        Listener = GameObject.FindObjectOfType<AudioListener>();
    }
}
