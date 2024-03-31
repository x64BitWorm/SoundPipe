using System;

namespace SP.UI.Components.StatusBar
{
    public class StatusBarMessagePipe
    {
        public delegate void StatusBarMessage(string message, StatusMessageType type,
            bool temporary, string linkText, Action linkAction);

        private StatusBarMessage _listener;

        public void RegisterListener(StatusBarMessage listener)
        {
            _listener += listener;
        }

        public void PushMessage(string message, StatusMessageType type = StatusMessageType.Info,
            bool temporary = false, string linkText = "", Action linkAction = null)
        {
            if (_listener == null)
            {
                return;
            }
            _listener(message, type, temporary, linkText, linkAction);
        }
    }
}
