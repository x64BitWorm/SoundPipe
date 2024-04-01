using SP.SDK.Primitives.Types;
using SP.SDK.Primitives;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SP.UI.Components.PropertiesStore.Types
{
    public class ImageType: PropertyType
    {
        private BitmapImage _output;

        public BitmapImage Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        public override void SetDefaultValue(object value)
        {
            Output = BitmapToImageSource(value as Bitmap);
        }

        public static ImageType FromParameterType(DynamicParameter parameter, DynamicImageType imageType)
        {
            return new ImageType();
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
