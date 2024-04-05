using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private AudioClip[] _damagedClips;
    [SerializeField] private AudioClip[] _diedClips;

    private MeshRenderer[] _bodyParts;
    private AudioSourceWrapper _damagedSource;
    private AudioSourceWrapper _diedSource;
    private float _blinkTime = 0.2f;
    private float _dissolveTime = 3f;

    public event Action<Character> Dissolved;

    private void Awake()
    {
        FillBodyPartsList();

        _damagedSource = AddAudioSource(_damagedClips);
        _diedSource = AddAudioSource(_diedClips);
    }

    private void FillBodyPartsList()
    {
        _bodyParts = GetComponentsInChildren<MeshRenderer>();
    }

    private AudioSourceWrapper AddAudioSource(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            AudioSourceWrapper result = new AudioSourceWrapper(transform, true);
            result.SetClip(clips);
            return result;
        }
        else
        {
            return null;
        }
    }

    private void OnDestroy()
    {
        _damagedSource = null;
        _diedSource = null;

        DOTween.Kill(this);
    }

    public void OnDamaged(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        if (_damagedSource != null)
        {
            _damagedSource.Play();
        }

        foreach (var part in _bodyParts)
        {
            DOTween.Sequence()
                .Append(part.materials[0].DOColor(Color.red, _blinkTime / 2))
                .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2));
        }
    }

    public void OnDied()
    {
        if (_diedSource != null)
        {
            _diedSource.Play();
        }

        foreach (var part in _bodyParts)
        {
            Collider collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                part.GetComponent<Collider>().enabled = true;
                part.AddComponent<Rigidbody>();
            }
        }
    }

    public void Dissolve()
    {
        foreach (var part in _bodyParts)
        {
            DOTween.Sequence()
                .Append(part.transform.DOScale(0f, _dissolveTime))
                .OnComplete(() => Dissolved?.Invoke(GetComponent<Character>()));
        }
    }
}
