using SP.UI.Views;
using System;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void DefaultMenuOpening(System.Drawing.Point point)
        {
            GraphContextMenu = _contextMenuBuilder.CreateDefaultMenu(f => AddNewNode(f, point));
        }

        public void NodeMenuOpening(string nodeId)
        {
            GraphContextMenu = _contextMenuBuilder.CreateNodeMenu(nodeId, RenameNode, RemoveNode);
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
            _shemeConstructor.RemoveFilter(nodeId);
            SyncGraphConstructorToUiGraph();
            NotifyShemeModified();
        }

        private void AddNewNode(string filterName, System.Drawing.Point point)
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
