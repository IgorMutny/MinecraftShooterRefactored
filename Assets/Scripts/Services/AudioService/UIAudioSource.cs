using UnityEngine;

public class UIAudioSource : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClicked;

    private AudioSourceWrapper _buttonSource;

    public void Initialize(AudioService audioService)
    {
        _buttonSource = new AudioSourceWrapper(gameObject, audioService);
        _buttonSource.IgnoreListenerPause(true);
        _buttonSource.SetClip(_buttonClicked);
    }

    public void OnButtonClicked()
    {
        _buttonSource.Play();
    }
}
