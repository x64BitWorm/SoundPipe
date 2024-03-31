namespace SP.UI.Components.GraphViewer.Interaction
{
    public class Collision
    {
        public int X {  get; set; }
        public int Y {  get; set; }

        public Collision(System.Drawing.Point point)
        {
            X = point.X;
            Y = point.Y;
        }
    }
}
