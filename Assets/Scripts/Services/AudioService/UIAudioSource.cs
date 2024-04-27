using UnityEngine;

public class UIAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClicked;

    private AudioSourceWrapper _buttonSource;
    private AudioSourceWrapper _additionalSource;
    private AudioSourceWrapper _musicSource;

    public void Initialize(AudioService audioService)
    {
        _buttonSource = new AudioSourceWrapper(transform, audioService);
        _buttonSource.IgnoreListenerPause(true);
        _buttonSource.SetClip(_buttonClicked);

        _additionalSource = new AudioSourceWrapper(transform, audioService);
        _additionalSource.IgnoreListenerPause(false);

        _musicSource = new AudioSourceWrapper(transform, audioService, false, true);
        _musicSource.IgnoreListenerPause(false);
    }

    public void OnButtonClicked()
    {
        _buttonSource.Play();
    }

    public void Play(AudioClip clip)
    {
        _additionalSource.SetClip(clip);
        _additionalSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.SetClip(clip);
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void UpdateMusicVolume()
    {
        _musicSource.ResetVolumeImmediately();
    }
}
