namespace SP.SDK.Primitives.Types
{
    public class DynamicImageType : IDynamicType
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public DynamicImageType(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public object FromString(string from)
        {
            throw new InvalidOperationException("IMAGE cannot be used as parameter in constructor");
        }

        public Type GetValueType()
        {
            return typeof(System.Drawing.Bitmap);
        }

        public bool IsValidObject(object value)
        {
            return value is System.Drawing.Bitmap bmp && bmp.Width == Width && bmp.Height == Height;
        }

        public string ToString(object from)
        {
            var bitmap = from as System.Drawing.Bitmap;
            return $"Image ({bitmap.Width}x{bitmap.Height})";
        }
    }
}
