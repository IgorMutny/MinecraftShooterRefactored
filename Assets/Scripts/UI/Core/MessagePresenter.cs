using UnityEngine;

namespace CoreUIElements
{
    public class MessagePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _messageSample;

        public void ShowMessage(string text, Color color)
        {
            Message message = Instantiate(_messageSample, transform).GetComponent<Message>();
            message.Initialize(text, color);
        }
    }
}