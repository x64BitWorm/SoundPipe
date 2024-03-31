using System.Drawing;

namespace SP.UI.Utils
{
    public static class GeomUtils
    {
        public static Point Add(this Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point Sub(this Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point Middle(this Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public static bool RectIntersects(Rectangle rectangle, Point point)
        {
            return point.X >= rectangle.Left && point.X <= rectangle.Right &&
                point.Y >= rectangle.Top && point.Y <= rectangle.Bottom;
        }

        public static Point RectCenter(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }
    }
}
