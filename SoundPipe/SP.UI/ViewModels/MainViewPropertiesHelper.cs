using SP.UI.Utils;
using System;
using System.Linq;
using System.Windows.Threading;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        private string _selectedNode;
        private DispatcherTimer _updateTimer;

        public void NodePropertiesOpening(string nodeId)
        {
            _selectedNode = nodeId;
            if (GetShemeState() == Models.ShemeState.Constructor)
            { 
                var filterParameters = _shemeConstructor.GetFilterParameters(_selectedNode);
                var viewParameters = _propertiesViewManager.ConvertToViewTypes(filterParameters).ToArray();
                _propertiesViewManager.ApplyDefaults(viewParameters, filterParameters
                    .Select(fp => _shemeConstructor.GetParameterValue(_selectedNode, fp.Name))
                    .ToArray());
                PropertyTypes = viewParameters.ToArray();
            }
            else if (GetShemeState() == Models.ShemeState.Running)
            {
                var shemeParameters = _pipeSheme.ListHotParameters(_selectedNode);
                var viewParameters = _propertiesViewManager.ConvertToViewTypes(shemeParameters).ToArray();
                _propertiesViewManager.ApplyDefaults(viewParameters, shemeParameters
                    .Select(fp => fp.ParameterType == SDK.Primitives.HotParameterType.WriteOnly
                    ? null : _pipeSheme.GetVar(_selectedNode, fp.Name))
                    .ToArray());
                var readOnly = shemeParameters
                        .Where(sp => sp.ParameterType == SDK.Primitives.HotParameterType.ReadOnly)
                        .ToArray();
                var readOnlyNames = readOnly.Select(p => p.Name).ToArray();
                var readOnlyView = viewParameters.Where(vp => readOnlyNames.Contains(vp.Id)).ToArray();
                SetupPropertyUpdaterTimer(() =>
                {
                    try
                    {
                        _propertiesViewManager.ApplyDefaults(readOnlyView, readOnly
                            .Select(fp => _pipeSheme.GetVar(_selectedNode, fp.Name))
                            .ToArray());
                    }
                    catch
                    { }
                });
                PropertyTypes = viewParameters.ToArray();
            }
        }

        public void PropertyChangedEvent(string parameterName, object value)
        {
            if (GetShemeState() == Models.ShemeState.Constructor)
            {
                _shemeConstructor.SetParameterValue(_selectedNode, parameterName, value);
            }
            else if (GetShemeState() == Models.ShemeState.Running)
            {
                _pipeSheme.SetVar(_selectedNode, parameterName, value);
            }
            NotifyShemeModified();
        }

        private void SetupPropertyUpdaterTimer(Action updater)
        {
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
            }
            _updateTimer = new DispatcherTimer();
            _updateTimer.Tick += new EventHandler(async (o, a) =>
            {
                await _uiContext;
                updater.Invoke();
            });
            _updateTimer.Interval = TimeSpan.FromMilliseconds();
            _updateTimer.Start();
        }
    }
}
