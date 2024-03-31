using System.Linq;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        private string _selectedNode;

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
                var shemeParameters = _pipeSheme.ListReadWriteParameters(_selectedNode);
                var viewParameters = _propertiesViewManager.ConvertToViewTypes(shemeParameters).ToArray();
                _propertiesViewManager.ApplyDefaults(viewParameters, shemeParameters
                    .Select(fp => _pipeSheme.GetVar(_selectedNode, fp.Name))
                    .ToArray());
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
    }
}
