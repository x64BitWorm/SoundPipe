using System.Drawing;

namespace SP.UI.Components.GraphViewer.Interaction
{
    internal class LinkCollision : Collision
    {
        public NodeContainer Container { get; set; }
        public int LinkNumber { get; set; }

        public LinkCollision(Point point) : base(point) { }
    }
}
