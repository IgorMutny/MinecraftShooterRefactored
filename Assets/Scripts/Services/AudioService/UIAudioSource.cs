using UnityEngine;

public class UIAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClicked;

    private AudioSourceWrapper _buttonSource;
    private AudioSourceWrapper _additionalSource;

    public void Initialize(AudioService audioService)
    {
        _buttonSource = new AudioSourceWrapper(transform, audioService);
        _buttonSource.IgnoreListenerPause(true);
        _buttonSource.SetClip(_buttonClicked);

        _additionalSource = new AudioSourceWrapper(transform, audioService);
        _additionalSource.IgnoreListenerPause(false);
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
}
