using SP.SDK;
using SP.SDK.Primitives;
using System.Drawing;

namespace SP.Filters.WaveVisualizer
{
    public class WaveVisualizer: ISoundFilter
    {
        private ISoundProvider _provider;
        private SoundData _lastData;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
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
            throw new InvalidOperationException("WAVEVISUALIZER filter has no 'set' parameters");
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
            var penL = new Pen(Color.Red);
            var penR = new Pen(Color.Green);
            var hMid = bmp.Height / 2;
            var w = bmp.Width;
            for (int x = 0; x < w; x++)
            {
                var sample = _lastData[x * _lastData.Length / w];
                g.DrawRectangle(penL, x, (int)(hMid + hMid * sample.left), 2, 2);
                g.DrawRectangle(penR, x, (int)(hMid + hMid * sample.right), 2, 2);
            }
        }
    }
}
