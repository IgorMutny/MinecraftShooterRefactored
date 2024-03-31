using DG.Tweening;
using UnityEngine;

namespace CoreUIElements
{
    public class YouDeadMessage : MonoBehaviour
    {
        public void Animate()
        {
            RectTransform rect = GetComponent<RectTransform>();

            DOTween.Sequence()
            .Append(rect.DOAnchorMax(new Vector2(0.5f, 0.75f), 3f))
            .Insert(0f, rect.DOAnchorMin(new Vector2(0.5f, 0.75f), 3f));
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}