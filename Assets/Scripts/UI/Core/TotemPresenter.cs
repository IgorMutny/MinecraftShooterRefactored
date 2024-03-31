using DG.Tweening;
using UnityEngine;

namespace CoreUIElements
{
    public class TotemPresenter : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject, 3f);

            DOTween.Sequence()
                .Append(transform.DOLocalMoveZ(2, 0.25f))
                .Insert(0.25f, transform.DOLocalRotate(new Vector3(0, 15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(0.5f, transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f)).SetEase(Ease.InSine)
                .Insert(0.75f, transform.DOLocalRotate(new Vector3(0, -15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(1f, transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f)).SetEase(Ease.InSine)
                .Insert(1.25f, transform.DOLocalRotate(new Vector3(0, 15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(1.5f, transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f)).SetEase(Ease.InSine)
                .Insert(1.75f, transform.DOLocalRotate(new Vector3(0, -15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(2f, transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f)).SetEase(Ease.InSine)
                .Insert(2.25f, transform.DOLocalRotate(new Vector3(0, 15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(2.5f, transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f)).SetEase(Ease.InSine)
                .Insert(2.75f, transform.DOLocalRotate(new Vector3(0, -15, 0), 0.25f)).SetEase(Ease.OutSine)
                .Insert(2.5f, transform.DOScale(0, 0.5f));

        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}