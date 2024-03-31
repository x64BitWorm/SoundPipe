using System;

namespace SP.UI.Components.StatusBar
{
    internal class StatusBarMessage
    {
        public string Message { get; set; }
        public StatusMessageType Type { get; set; }
        public string LinkText { get; set; }
        public Action LinkAction { get; set; }

        public StatusBarMessage(): this("", StatusMessageType.Default, "", null) { }

        public StatusBarMessage(string message, StatusMessageType type, string linkText,
            Action linkAction)
        {
            Message = message;
            Type = type;
            LinkText = linkText;
            LinkAction = linkAction;
        }
    }
}
