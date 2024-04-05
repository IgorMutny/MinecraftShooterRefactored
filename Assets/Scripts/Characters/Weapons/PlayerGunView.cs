using DG.Tweening;
using System;
using UnityEngine;

public class PlayerGunView : MonoBehaviour
{
    private readonly string _shootKey = "Shoot";
    private readonly string _reloadKey = "Reload";
    private readonly string _isEmptyKey = "IsEmpty";

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private ParticleSystem _gunSmoke;

    private MeshRenderer[] _weaponParts;
    private Animator _animator;
    private AudioSourceWrapper _shootSource;
    private AudioSourceWrapper _reloadSource;

    private float _innerVolume = 0.5f;
    private float _changeColorTime = 0.2f;

    public event Action Removed;

    private void Awake()
    {
        FillWeaponPartsList();

        _animator = GetComponent<Animator>();

        if (_shootClip != null)
        {
            _shootSource = new AudioSourceWrapper(transform, true);
            _shootSource.SetClip(_shootClip);
            _shootSource.SetVolume(_innerVolume);
        }

        if (_reloadClip != null)
        {
            _reloadSource = new AudioSourceWrapper(transform, true);
            _reloadSource.SetClip(_reloadClip);
            _shootSource.SetVolume(_innerVolume);
        }
    }

    private void FillWeaponPartsList()
    {
        _weaponParts = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetEmpty()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_isEmptyKey);
        }
    }

    public void Shoot()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_shootKey);
        }

        if (_gunSmoke != null)
        {
            _gunSmoke.Play();
        }

        if (_shootSource != null)
        {
            _shootSource.Play();
        }
    }

    public void Reload()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_reloadKey);
        }

        if (_reloadSource != null)
        {
            _reloadSource.Play();
        }
    }

    public void Remove(float time)
    {
        DOTween.Sequence()
            .Append(transform.DOLocalRotate(new Vector3(60, 0, 0), time))
            .OnComplete(() => Removed?.Invoke());
    }

    public void Raise(float time)
    {
        DOTween.Sequence()
                .Append(transform.DOLocalRotate(new Vector3(60, 0, 0), 0))
                .Append(transform.DOLocalRotate(new Vector3(0, 0, 0), time));
    }

    public void ChangeSpeed(float multiplier)
    {
        if (_animator != null)
        {
            _animator.speed = multiplier;
        }

        if (_shootSource != null)
        {
            _shootSource.SetSpeed(multiplier);
        }

        if (_reloadSource != null)
        {
            _reloadSource.SetSpeed(multiplier);
        }
    }

    public void ChangeDamage(float multiplier)
    {
        if (multiplier > 1)
        {
            ChangeColor(Color.red);
        }

        if (multiplier == 1)
        {
            ChangeColor(Color.white);
        }
    }

    private void ChangeColor(Color color)
    {
        foreach (MeshRenderer part in _weaponParts)
        {
            foreach (Material material in part.materials)
            {
                material.DOColor(color, _changeColorTime);
            }
        }
    }

    private void OnDestroy()
    {
        _shootSource = null;
        _reloadSource = null;

        DOTween.Kill(this);
    }
}
