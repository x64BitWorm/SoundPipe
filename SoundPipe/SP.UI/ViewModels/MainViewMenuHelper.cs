using SP.UI.Utils;
using SP.UI.Views;
using System;
using System.Drawing;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void DefaultMenuOpening(Point point)
        {
            if (IsShemeReadOnly)
            {
                GraphContextMenu = null;
                return;
            }
            GraphContextMenu = _contextMenuBuilder.CreateDefaultMenu(f => AddNewNode(f, point));
        }

        public void NodeMenuOpening(string nodeId)
        {
            if (IsShemeReadOnly)
            {
                GraphContextMenu = null;
                return;
            }
            GraphContextMenu = _contextMenuBuilder.CreateNodeMenu(nodeId, RenameNode, RemoveNode, ChangeBorderNode);
        }

        private void RenameNode(string nodeId)
        {
            SyncUiGraphToGraphConstructor();
            var oldLabel = _shemeConstructor[nodeId].MetaInfo.Label;
            var newName = InputDialog.ShowAndGetResult("Новое имя фильтра", oldLabel);
            _shemeConstructor[nodeId].MetaInfo.Label = newName;
            SyncGraphConstructorToUiGraph();
        }

        private void RemoveNode(string nodeId)
        {
            SyncUiGraphToGraphConstructor();
            if (_selectedNode == nodeId)
            {
                PropertyTypes = Array.Empty<Components.PropertiesStore.PropertyType>();
            }
            _shemeConstructor.RemoveFilter(nodeId);
            SyncGraphConstructorToUiGraph();
            NotifyShemeModified();
        }

        private void ChangeBorderNode(string nodeId, System.Windows.Media.Color borderColor)
        {
            SyncUiGraphToGraphConstructor();
            _shemeConstructor[nodeId].MetaInfo.Color = WpfUtils.ColorToString(WpfUtils.DrawingColorFromMediaColor(borderColor));
            SyncGraphConstructorToUiGraph();
        }

        private void AddNewNode(string filterName, Point point)
        {
            SyncUiGraphToGraphConstructor();
            _shemeConstructor.AddFilter(Guid.NewGuid().ToString(), filterName, new Models.UINodeInfo()
            {
                Label = filterName,
                X = point.X,
                Y = point.Y,
                Color = "Black"
            });
            SyncGraphConstructorToUiGraph();
            NotifyShemeModified();
        }
    }
}
