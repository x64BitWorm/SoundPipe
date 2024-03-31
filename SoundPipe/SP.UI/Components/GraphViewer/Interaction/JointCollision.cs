namespace SP.UI.Components.GraphViewer.Interaction
{
    public class JointCollision: Collision
    {
        public NodeContainer Container { get; set; }
        public bool IsInput { get; set; }
        public int LinkNumber { get; set; }

        public JointCollision(System.Drawing.Point point): base(point) { }
    }
}
