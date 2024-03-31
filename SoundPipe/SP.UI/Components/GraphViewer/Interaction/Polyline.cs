using System.Drawing;

namespace SP.UI.Components.GraphViewer.Interaction
{
    public class Polyline
    {
        public Point[] Points { get; }
        public int Width { get; }

        public Polyline(Point[] points, int width)
        {
            Points = points;
            Width = width;
        }

        public bool Intersects(Point point)
        {
            if (Points.Length == 0)
            {
                return false;
            }
            var from = Points[0];
            for (int i = 1; i < Points.Length; i++)
            {
                var to = Points[i];
                if (Intersects(from, to, point))
                {
                    return true;
                }
                from = to;
            }
            return false;
        }

        private bool Intersects(Point from, Point to, Point point)
        {
            var horizontal = from.Y == to.Y;
            if (horizontal)
            {
                return InRange(from.X, to.X, point.X) && InRange(from.Y - Width, to.Y + Width, point.Y);
            }
            return InRange(from.Y, to.Y, point.Y) && InRange(from.X - Width, to.X + Width, point.X);
        }

        private bool InRange(int x, int y, int value)
        {
            if (y > x)
            {
                return value >= x && value <= y;
            }
            return value <= x && value >= y;
        }
    }
}
