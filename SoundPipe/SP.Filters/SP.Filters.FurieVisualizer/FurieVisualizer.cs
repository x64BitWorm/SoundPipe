using SP.SDK;
using SP.SDK.Primitives;
using System.Drawing;

namespace SP.Filters.FurieVisualizer
{
    public class FurieVisualizer: ISoundFilter
    {
        private ISoundProvider _provider;
        private int _zoom;
        private SoundData _lastData;
        private readonly Font _textFont = 
            new Font(SystemFonts.DefaultFont.FontFamily, 24, GraphicsUnit.Pixel);

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is int zoom)
            {
                _zoom = zoom;
            }
            else
            {
                throw new ArgumentException("Arg2: Int expected");
            }
            _lastData = new SoundData(1);
        }

        public SoundData ReadPart(int length)
        {
            var result = _provider.ReadPart(length);
            _lastData = result.Clone();
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException("FURIEVISUALIZER filter has no 'set' parameters");
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "image":
                    var output = new Bitmap(512, 512);
                    DrawWave(output);
                    return output;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'image' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }

        private void DrawWave(Bitmap bmp)
        {
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var penF = new Pen(Color.Blue);
            var hMid = bmp.Height / 2;
            var w = bmp.Width;
            var h = bmp.Height;
            var lastSamples = _lastData.samples.Select(s => (s.left + s.right) / 2.0).ToArray();
            var fft = FFT.Transform(lastSamples);
            for (int x = 0; x < w; x++)
            {
                var value = fft[(int)((float)x / w * fft.Length)] * _zoom;
                g.DrawRectangle(penF, x, (int)(h - value * h), 1, (int)(value * h));
            }
            g.DrawString($"{1} - {fft.Length}", _textFont, Brushes.Black, new Point(5, 5));
        }
    }
}
