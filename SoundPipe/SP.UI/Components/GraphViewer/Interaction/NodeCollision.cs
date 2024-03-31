namespace SP.UI.Components.GraphViewer.Interaction
{
    public class NodeCollision: Collision
    {
        public NodeContainer Container { get; set; }

        public NodeCollision(System.Drawing.Point point): base(point) { }
    }
}
