using DG.Tweening;
using System;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    private readonly string _shootKey = "Shoot";
    private readonly string _reloadKey = "Reload";

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private ParticleSystem _gunSmoke;

    private Animator _animator;
    private AudioSourceWrapper _shootSource;
    private AudioSourceWrapper _reloadSource;

    public event Action Removed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _shootSource = new AudioSourceWrapper(gameObject);
        _shootSource.SetClip(_shootClip);
        _reloadSource = new AudioSourceWrapper(gameObject);
        _reloadSource.SetClip(_reloadClip);
    }

    public void Shoot()
    {
        _animator.SetTrigger(_shootKey);
        _gunSmoke.Play();
        _shootSource.Play();
    }

    public void Reload()
    {
        _animator.SetTrigger(_reloadKey);
        _reloadSource.Play();
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

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
