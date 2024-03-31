using SP.UI.Components.GraphViewer.Interaction;
using SP.UI.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SP.UI.Components.GraphViewer
{
    public partial class GraphViewer
    {
        private void MeasureNodes(IEnumerable<NodeContainer> nodes)
        {
            foreach (var container in nodes)
            {
                var node = container.Node;
                var areaHeight = Math.Max(60, Math.Max(node.Inputs.Length, node.Outputs.Length) * 15 + 15);
                var area = new Rectangle(node.X - _viewOrigin.X, node.Y - _viewOrigin.Y, 100, areaHeight);
                container.Area = area;
                container.InputsArea = node.Inputs
                    .Select((l, i) => new Rectangle(area.Left - 5, area.Y + 10 + i * 15, 10, 10)).ToArray();
                container.OutputsArea = node.Outputs
                    .Select((l, i) => new Rectangle(area.Right - 5, area.Y + 10 + i * 15, 10, 10)).ToArray();
            }
            foreach (var container in nodes)
            {
                container.InputsLinks = container.Node.Inputs
                    .Select((l, i) => l.Destination == null ? null : 
                    MeasureLink(GeomUtils.RectCenter(_nodes[l.Destination].OutputsArea[l.Index]),
                    GeomUtils.RectCenter(container.InputsArea[i])))
                    .Where(l => l != null)
                    .ToArray();
            }
        }

        private Polyline MeasureLink(Point from, Point to)
        {
            if (to.X - from.X > 40)
            {
                var middleX = (from.X + to.X) / 2;
                return new Polyline(new Point[] {
                    from,
                    new Point(middleX, from.Y),
                    new Point(middleX, to.Y),
                    to
                }, 2);
            }
            int posY = Math.Abs(from.Y - to.Y) < 60
                ? Math.Max(from.Y, to.Y) - 40
                : (from.Y + to.Y) / 2;
            return new Polyline(new Point[] {
                from,
                new Point(from.X + 20, from.Y),
                new Point(from.X + 20, posY),
                new Point(to.X - 20, posY),
                new Point(to.X - 20, to.Y),
                to
            }, 2);
        }
    }
}
