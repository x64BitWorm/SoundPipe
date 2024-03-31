using SP.UI.Components.GraphViewer.Interaction;
using SP.UI.Utils;
using System.Drawing;
using System.Threading.Tasks;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer
    {
        private static Font NodeLabelFont = new Font(SystemFonts.DefaultFont.FontFamily, 20, GraphicsUnit.Pixel);
        private static Font TooltipFont = new Font(SystemFonts.DefaultFont.FontFamily, 18, GraphicsUnit.Pixel);
        private static Brush TooltipFillColor = Brushes.WhiteSmoke;
        private static Color Background = Color.White;
        private static Color GridDotColor = Color.LightSlateGray;
        private static Color NodeLinkColor = Color.Black;
        private static Brush NodeFillColor = Brushes.LightGray;
        private static Color NodeDefaultBorder = Color.Black;
        private static Color FontColor = Color.Black;

        private void DrawGraphNode(NodeContainer container)
        {
            var node = container.Node;
            var linkPen = new SolidBrush(NodeLinkColor);
            var borderColor = new Pen(node.BorderColor, 2);
            var fontBrush = new SolidBrush(FontColor);
            var area = container.Area;
            var textArea = new Rectangle(area.X + 5, area.Y + 5, area.Width - 10, area.Height - 10);
            _canva.DrawRoundedRectangle(borderColor, area, 20, NodeFillColor);
            _canva.DrawString(node.Label, NodeLabelFont, fontBrush, textArea);
            for (int i = 0; i < node.Inputs.Length; i++)
            {
                _canva.FillEllipse(linkPen, container.InputsArea[i]);
            }
            for (int i = 0; i < node.Outputs.Length; i++)
            {
                _canva.FillEllipse(linkPen, container.OutputsArea[i]);
            }
        }

        private void DrawGraphLinks(NodeContainer container)
        {
            foreach (var link in container.InputsLinks)
            {
                var pen = new Pen(NodeLinkColor, link.Width);
                _canva.DrawLines(pen, link.Points);
            }
        }

        private void DrawStateAdornments()
        {
            if (_state.State == Events.StateType.ReconnectLink)
            {
                var nodeJoint = _state.Context.Argument as NodeJoint;
                var jointPosition = GetJointPosition(nodeJoint);
                var pen = new Pen(NodeLinkColor, 2);
                _canva.DrawLine(pen, jointPosition, _lastMousePosition);
            }
            if (_selectedNode != null)
            {
                var pen = new Pen(_selectedNode.Node.BorderColor, 4);
                _canva.DrawRoundedRectangle(pen, _selectedNode.Area, 20);
            }
        }

        private async Task DrawTooltipsAsync()
        {
            await _uiContext;
            var text = string.Empty;
            var collision = GetCollision(_lastMousePosition);
            if (collision is JointCollision collisionJoint)
            {
                text = GetNodeJoint(new NodeJoint()
                {
                    Node = collisionJoint.Container.Node,
                    IsInput = collisionJoint.IsInput,
                    Index = collisionJoint.LinkNumber
                }).Description;
            }
            else if (collision is LinkCollision collisionLink)
            {
                var from = GetOutputSide(collisionLink.Container.Node, collisionLink.LinkNumber);
                text = $"{from.Node.Label} → {collisionLink.Container.Node.Label}";
            }
            else if (collision is NodeCollision collisionNode)
            {
                text = collisionNode.Container.Node.Popup;
            }
            if (text != string.Empty)
            {
                var size = _canva.MeasureString(text, TooltipFont);
                var position = GeomUtils.Add(_lastMousePosition, new Point(20, 25));
                var area = new Rectangle(position, new Size((int)size.Width, (int)size.Height));
                _canva.FillRectangle(TooltipFillColor, area);
                _canva.DrawRectangle(new Pen(NodeDefaultBorder), area);
                _canva.DrawString(text, TooltipFont, new SolidBrush(FontColor), position);
                UpdateGraphImage();
            }
        }

        private void DrawGrid()
        {
            var pen = new Pen(GridDotColor);
            var width = _canvaBitmap.Width;
            var height = _canvaBitmap.Height;
            var gridStep = 50;
            for (int y = -(_viewOrigin.Y % gridStep); y < height; y += gridStep)
            {
                for (int x = -(_viewOrigin.X % gridStep); x < width; x += gridStep)
                {
                    _canva.DrawEllipse(pen, x, y, 2, 2);
                }
            }
        }
    }
}
