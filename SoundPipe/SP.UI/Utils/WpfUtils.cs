using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SP.UI.Utils
{
    public static class WpfUtils
    {
        public static System.Windows.Media.ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero,
                    Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        public static System.Windows.Size GetElementSizeInPixels(FrameworkElement element)
        {
            var source = PresentationSource.FromVisual(element);
            var transformToDevice = source.CompositionTarget.TransformToDevice;
            var sizeVecrtor = (Vector)new System.Windows.Size(element.ActualWidth, element.ActualHeight);
            return (System.Windows.Size)transformToDevice.Transform(sizeVecrtor);
        }

        public static System.Drawing.Point GetPositionInPixels(this FrameworkElement element)
        {
            GetCursorPos(out POINT p);
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            return new System.Drawing.Point((int)(p.X - absolutePos.X), (int)(p.Y - absolutePos.Y));
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rectangle, int cornerRadius, Brush? brush = null)
        {
            var cornerSize = new System.Drawing.Size(cornerRadius, cornerRadius);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(rectangle.Location, cornerSize), 180, 90);
            path.AddArc(new Rectangle(rectangle.Right - cornerRadius, rectangle.Top, cornerSize.Width, cornerSize.Height), 270, 90);
            path.AddArc(new Rectangle(rectangle.Right - cornerRadius, rectangle.Bottom - cornerRadius, cornerSize.Width, cornerSize.Height), 0, 90);
            path.AddArc(new Rectangle(rectangle.Left, rectangle.Bottom - cornerRadius, cornerSize.Width, cornerSize.Height), 90, 90);
            path.CloseFigure();
            if (brush != null)
            {
                graphics.FillPath(brush, path);
            }
            graphics.DrawPath(pen, path);
        }

        public static Color DrawingColorFromMediaColor(System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static string ColorToString(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Point DrawingPointFromWindowsPoint(System.Windows.Point point)
        {
            return new System.Drawing.Point((int)Math.Round(point.X), (int)Math.Round(point.Y));
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

    }
}
