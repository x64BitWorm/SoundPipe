using System;
using System.ComponentModel;

namespace SP.UI.Components.StatusBar
{
    internal class StatusBarContext: INotifyPropertyChanged
    {
        private string _iconEmoji;
        private string _title;
        private string _linkText;
        private Action _linkAction;

        public string IconEmoji
        {
            get => _iconEmoji;
            set
            {
                _iconEmoji = value;
                OnPropertyChanged(nameof(IconEmoji));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string LinkText
        {
            get => _linkText;
            set
            {
                _linkText = value;
                OnPropertyChanged(nameof(LinkText));
            }
        }

        public Action LinkAction
        {
            get => _linkAction;
            set
            {
                _linkAction = value;
                OnPropertyChanged(nameof(LinkAction));
            }
        }


        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
