using SP.UI.Components.GraphViewer.Events;
using SP.UI.Components.GraphViewer.Interaction;
using SP.UI.Utils;
using System.Drawing;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer
    {
        private StateContainer _state;
        private Point _lastMousePosition;
        private NodeContainer _selectedNode;

        private void CanvaMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var point = WpfUtils.GetPositionInPixels(Canva);
            _lastMousePosition = point;
            switch (_state.State)
            {
                case StateType.Empty:
                    _tooltipDebounce.Call();
                    break;
                case StateType.MoveOrigin:
                    _viewOrigin = GeomUtils.Sub(_state.Context.Position, point);
                    break;
                case StateType.ClickNode:
                    if (IsReadOnly)
                    {
                        return;
                    }
                    _state.State = StateType.MoveNode;
                    break;
                case StateType.MoveNode:
                    var container = _state.Context.Argument as NodeContainer;
                    var offset = GeomUtils.Sub(GeomUtils.Add(_viewOrigin, point), _state.Context.Position);
                    _nodes[container.Node.Id].Node.X = offset.X;
                    _nodes[container.Node.Id].Node.Y = offset.Y;
                    break;
            }
            _redrawDebounce.Call();
        }

        private void CanvaMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = WpfUtils.GetPositionInPixels(Canva);
            var collision = GetCollision(point);
            var absolutePosition = GeomUtils.Add(_viewOrigin, point);
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                return;
            }
            switch (_state.State)
            {
                case StateType.Empty:
                    if (collision is null)
                    {
                        _state.State = StateType.MoveOrigin;
                        _state.Context = new StateContext(absolutePosition, null);
                    }
                    else if (collision is NodeCollision nodeCollision)
                    {
                        _state.State = StateType.ClickNode;
                        var nodePosition = new Point(nodeCollision.Container.Node.X, nodeCollision.Container.Node.Y);
                        _state.Context = new StateContext(GeomUtils.Sub(absolutePosition, nodePosition), nodeCollision.Container);
                    }
                    else if (collision is JointCollision jointCollision)
                    {
                        if (IsReadOnly)
                        {
                            return;
                        }
                        var node = jointCollision.Container.Node;
                        var nodeJoint = new NodeJoint()
                        {
                            Node = node,
                            IsInput = jointCollision.IsInput,
                            Index = jointCollision.LinkNumber
                        };
                        _state.State = StateType.ReconnectLink;
                        var connectedTo = jointCollision.IsInput
                            ? GetOutputSide(node, jointCollision.LinkNumber)
                            : GetInputSide(node, jointCollision.LinkNumber);
                        if (connectedTo == null)
                        {
                            _state.Context = new StateContext(Point.Empty, nodeJoint);
                        }
                        else
                        {
                            _state.Context = new StateContext(Point.Empty, connectedTo);
                            RemoveJoint(nodeJoint);
                        }
                    }
                    _selectedNode = null;
                    break;
            }
        }

        private void CanvaMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = WpfUtils.GetPositionInPixels(Canva);
            var collision = GetCollision(point);
            var absolutePosition = GeomUtils.Add(_viewOrigin, point);
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                if (collision is NodeCollision nodeCollision)
                {
                    ContextOpeningInvoke(nodeCollision.Container.Node);
                }
                else
                {
                    ContextOpeningInvoke(null);
                }
                return;
            }
            switch (_state.State)
            {
                case StateType.ReconnectLink:
                    if (collision is JointCollision jointCollision)
                    {
                        var nodeJoint = new NodeJoint()
                        {
                            Node = jointCollision.Container.Node,
                            IsInput = jointCollision.IsInput,
                            Index = jointCollision.LinkNumber
                        };
                        AddJoint(_state.Context.Argument as NodeJoint, nodeJoint);
                    }
                    _state.State = StateType.Empty;
                    _state.Context = null;
                    break;
                case StateType.MoveNode:
                    _state.State = StateType.Empty;
                    _state.Context = null;
                    break;
                case StateType.ClickNode:
                    var node = (_state.Context.Argument as NodeContainer);
                    _state.State = StateType.Empty;
                    _state.Context = null;
                    _selectedNode = node;
                    NodeClickInvoke(node.Node);
                    RedrawAsync();
                    break;
                case StateType.MoveOrigin:
                    _state.State = StateType.Empty;
                    _state.Context = null;
                    break;
            }
        }

        private void NodeClickInvoke(GraphNode node)
        {
            var newEvent = new ParametrizedRoutedEventArgs(NodeClickEvent, this, node.Id);
            RaiseEvent(newEvent);
        }

        private void ContextOpeningInvoke(GraphNode node)
        {
            var args = new object[] { node == null ? string.Empty : node.Id, GeomUtils.Add(_lastMousePosition, _viewOrigin) };
            var newEvent = new ParametrizedRoutedEventArgs(ContextOpeningEvent, this, args);
            RaiseEvent(newEvent);
        }
    }
}
