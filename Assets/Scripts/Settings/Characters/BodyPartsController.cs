using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CharacterViewElements
{
    public class BodyPartsController
    {
        private MeshRenderer[] _bodyParts;
        private Transform _transform;

        private float _blinkTime = 0.2f;
        private float _dissolveTime = 3f;

        public event Action Dissolved;

        public BodyPartsController(Transform transform)
        {
            _transform = transform;
            _bodyParts = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public void OnDamaged()
        {
            foreach (var part in _bodyParts)
            {
                DOTween.Sequence()
                    .Append(part.materials[0].DOColor(Color.red, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2));
            }
        }

        public void Explode(float duration)
        {
            DOTween.Sequence()
                .Append(_transform.DOScale(1.5f, duration));

            foreach (MeshRenderer part in _bodyParts)
            {
                DOTween.Sequence()
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2))
                    .AppendInterval(duration * 0.15f)
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2))
                    .AppendInterval(duration * 0.1f)
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2))
                    .AppendInterval(duration * 0.075f)
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2))
                    .AppendInterval(duration * 0.05f)
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2))
                    .AppendInterval(duration * 0.025f)
                    .Append(part.materials[0].DOColor(Color.black, _blinkTime / 2))
                    .Append(part.materials[0].DOColor(Color.white, _blinkTime / 2));
            }

        }

        public void OnDied()
        {
            foreach (var part in _bodyParts)
            {
                Collider collider = part.GetComponent<Collider>();
                if (collider != null)
                {
                    part.transform.parent = null;
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
                    .OnComplete(() => Dissolved?.Invoke());
            }
        }

        public void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}