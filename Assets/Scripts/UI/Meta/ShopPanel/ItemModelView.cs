using UnityEngine;

namespace MetaUIElements
{
    public class ItemModelView : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed;

        private GameObject _currentModel;

        public void SetModel(GameObject model)
        {
            if (_currentModel != null)
            {
                Destroy(_currentModel);
            }

            _currentModel = Instantiate(model, transform.position, transform.rotation, (RectTransform)transform);
        }

        private void FixedUpdate()
        {
            transform.Rotate(_rotationSpeed);
        }
    }
}