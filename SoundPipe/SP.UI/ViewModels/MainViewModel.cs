using Microsoft.Extensions.DependencyInjection;
using SP.Domain;
using SP.Domain.Models;
using SP.UI.Components.GraphViewer;
using SP.UI.Components.PropertiesStore;
using SP.UI.Components.StatusBar;
using SP.UI.Models;
using SP.UI.Services;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;

namespace SP.UI.ViewModels
{
    public partial class MainViewModel: INotifyPropertyChanged
    {
        private readonly ShemeProvider _shemeProvider;
        private readonly ContextMenuBuilder _contextMenuBuilder;
        private readonly ShemeGraphConverter _shemeGraphConverter;
        private readonly PropertiesViewManager _propertiesViewManager;
        private readonly ShemeManager _shemeManager;
        private readonly IServiceProvider _serviceProvider;

        private SettingsModel _settingsModel;
        private SynchronizationContext _uiContext;
        private ShemeConstructor<UINodeInfo> _shemeConstructor;
        private PipeSheme _pipeSheme;

        private GraphNode[] _shemeNodes;
        private ContextMenu _graphContextMenu;
        private StatusBarMessagePipe _statusBarSend;
        private PropertyType[] _propertyTypes;
        private string _windowTitle;

        public GraphNode[] ShemeNodes
        {
            get => _shemeNodes;
            set
            {
                _shemeNodes = value;
                OnPropertyChanged(nameof(ShemeNodes));
            }
        }

        public ContextMenu GraphContextMenu
        {
            get => _graphContextMenu;
            set
            {
                _graphContextMenu = value;
                OnPropertyChanged(nameof(GraphContextMenu));
            }
        }

        public StatusBarMessagePipe StatusBarSend
        {
            get => _statusBarSend;
            set
            {
                _statusBarSend = value;
                OnPropertyChanged(nameof(StatusBarSend));
            }
        }

        public PropertyType[] PropertyTypes
        {
            get => _propertyTypes;
            set
            {
                _propertyTypes = value;
                OnPropertyChanged(nameof(PropertyTypes));
            }
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }


        public MainViewModel(ShemeProvider shemeProvider, ContextMenuBuilder contextMenuBuilder,
            ShemeGraphConverter shemeGraphConverter, PropertiesViewManager propertiesViewManager,
            ShemeManager shemeManager, IServiceProvider serviceProvider, SettingsProvider settingsProvider)
        {
            _shemeProvider = shemeProvider;
            _contextMenuBuilder = contextMenuBuilder;
            _shemeGraphConverter = shemeGraphConverter;
            _propertiesViewManager = propertiesViewManager;
            _shemeManager = shemeManager;
            _serviceProvider = serviceProvider;
            _settingsModel = settingsProvider.Load();
            CreateEmptyShemeMenu();
            StatusBarSend = new StatusBarMessagePipe();
        }

        public void ViewAttached()
        {
            _uiContext = SynchronizationContext.Current;
            StatusBarSend.PushMessage("Добро пожаловать в SoundPipe!", StatusMessageType.Info, true);
        }

        public ShemeState GetShemeState()
        {
            return _pipeSheme != null ? ShemeState.Running : ShemeState.Constructor;
        }

        public void StopPipeSheme()
        {
            if (_pipeSheme != null)
            {
                _pipeSheme.Destroy();
            }
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer = null;
            }
            _pipeSheme = null;
        }

        public void UpdateTitle()
        {
            WindowTitle = $"Sound Pipe [{_openedFileName ?? "New"}]";
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
