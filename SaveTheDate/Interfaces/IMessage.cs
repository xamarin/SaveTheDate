using System;

namespace SaveTheDate.Interfaces
{
    public interface IMessage
    {
        void SendMessage(string message, string title = null);
        void SendToast(string message);
        void SendConfirmation(string message, string title, Action<bool> confirmationAction);
    }
}

