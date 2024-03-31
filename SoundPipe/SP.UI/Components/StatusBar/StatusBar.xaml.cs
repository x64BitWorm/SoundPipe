using SP.UI.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SP.UI.Components.StatusBar
{
    public partial class StatusBar : UserControl
    {
        public static readonly DependencyProperty MessageSendProperty =
            DependencyProperty.Register(nameof(MessageSend), typeof(StatusBarMessagePipe), typeof(StatusBar),
                new PropertyMetadata(null, OnMessageSendChanged));

        private SynchronizationContext _uiContext;

        private StatusBarMessage _currentMessage;
        private StatusBarMessage _defaultMessage;

        public StatusBarMessagePipe MessageSend
        {
            get => (StatusBarMessagePipe)GetValue(MessageSendProperty);
            set => SetValue(MessageSendProperty, value);
        }

        private readonly StatusBarContext _context;

        public StatusBar()
        {
            _uiContext = SynchronizationContext.Current;
            _context = new StatusBarContext();
            _defaultMessage = new StatusBarMessage();
            InitializeComponent();
        }

        private static void OnMessageSendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (StatusBar)d;
            var value = e.NewValue as StatusBarMessagePipe;
            if (value != null)
            {
                value.RegisterListener((me, ty, te, lt, la) => source.MessageReceived(me, ty, te, lt, la));
            }
        }

        private void UpdateMessage(StatusBarMessage message)
        {
            _context.Title = message.Message;
            _context.LinkText = message.LinkText;
            _context.LinkAction = message.LinkAction;
            _context.IconEmoji = MessageTypeToEmoji(message.Type);
        }

        private async Task MessageReceived(string message, StatusMessageType type, bool temporary, string linkText, Action linkAction)
        {
            await _uiContext;
            var statusMessage = new StatusBarMessage(message, type, linkText, linkAction);
            MainAnimation.Storyboard.Begin();
            await Task.Run(async () =>
            {
                await Task.Delay(200);
                UpdateMessage(statusMessage);
                _currentMessage = statusMessage;
                if (temporary)
                {
                    await Task.Delay(3000);
                    await MessageReceived(_defaultMessage.Message, _defaultMessage.Type, false,
                        _defaultMessage.LinkText, _defaultMessage.LinkAction);
                }
                else
                {
                    _defaultMessage = statusMessage;
                }
            });
        }

        private void HyperlinkClick(object sender, RoutedEventArgs e)
        {
            _context.LinkAction?.Invoke();
        }

        private string MessageTypeToEmoji(StatusMessageType type)
        {
            switch (type)
            {
                case StatusMessageType.Error:
                    return "⛔";
                case StatusMessageType.Warning:
                    return "⚠️";
                case StatusMessageType.Info:
                    return "🛈";
                case StatusMessageType.Question:
                    return "❔";
                case StatusMessageType.Run:
                    return "▶️";
                case StatusMessageType.Success:
                    return "✔️ ";
                default:
                    return "";
            }
        }

        private void MainPanelLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _context;
        }
    }
}
