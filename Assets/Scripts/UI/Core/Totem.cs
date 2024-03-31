using UnityEngine;

namespace CoreUIElements
{
    public class Totem : MonoBehaviour
    {
        [SerializeField] private GameObject _image;
        [SerializeField] private GameObject _totemPresenterSample;

        public void SetActive(bool value)
        {
            _image.SetActive(value);
        }

        public void ShowResurrection(Transform head)
        {
            Instantiate(_totemPresenterSample, head);
        }
    }
}