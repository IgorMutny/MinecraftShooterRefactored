using CoreUIElements;
using UnityEngine;

public class MessageSender : IService
{
    private CoreUI _coreUI;

    public MessageSender(CoreUI coreUI)
    {
        _coreUI = coreUI;
    }

    public void ShowMessage(string text, Color color)
    {
        _coreUI.ShowMessage(text, color);
    }
}
