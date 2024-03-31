using SP.UI.Components.GraphViewer.Interaction;
using System.Drawing;

namespace SP.UI.Components.GraphViewer
{
    public class NodeContainer
    {
        public GraphNode Node { get; set; }
        public Rectangle Area { get; set; }
        public Rectangle[] InputsArea { get; set; }
        public Rectangle[] OutputsArea { get; set; }
        public Polyline[] InputsLinks { get; set; }

        public NodeContainer(GraphNode node)
        {
            Node = node;
        }
    }
}
