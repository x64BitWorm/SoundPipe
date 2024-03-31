using System.Drawing;

namespace SP.UI.Components.GraphViewer
{
    public class GraphNode
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Popup { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color BorderColor { get; set; }
        public GraphNodeLink[] Inputs { get; set; }
        public GraphNodeLink[] Outputs { get; set; }
    }
}
