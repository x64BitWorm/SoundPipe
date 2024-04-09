using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.StatsVisualizer
{
    public class StatsVisualizer : ISoundFilter
    {
        private ISoundProvider _provider;
        private int _totalSamples;
        private int _samplesSpeed;
        private int[] _lastTotalSamples;
        private Thread _updater;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _totalSamples = 0;
            _samplesSpeed = 0;
            _lastTotalSamples = new int[4];
            _updater = new Thread((e) => UpdateMetrics());
            _updater.Start();
        }

        public SoundData ReadPart(int length)
        {
            var samples = _provider.ReadPart(length);
            _totalSamples += samples.Length;
            return samples;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException($"STATSVISUALIZER has no SET parameters");
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "totalSamples":
                    return WithGroups(_totalSamples);
                case "samplesSpeed":
                    return WithGroups(_samplesSpeed);
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'totalSamples,samplesSpeed' is supported");
            }
        }

        public void Destroy()
        {
            _updater.Interrupt();
        }

        private void UpdateMetrics()
        {
            while (true)
            {
                for (int i = 0; i < _lastTotalSamples.Length - 1; i++)
                {
                    _lastTotalSamples[i] = _lastTotalSamples[i + 1];
                }
                _lastTotalSamples[_lastTotalSamples.Length - 1] = _totalSamples;
                var speed = 0;
                for (int i = 0; i < _lastTotalSamples.Length - 1; i++)
                {
                    speed += _lastTotalSamples[i + 1] - _lastTotalSamples[i];
                }
                _samplesSpeed = speed / (_lastTotalSamples.Length - 1);
                try
                {
                    Thread.Sleep(1000);
                }
                catch
                {
                    break;
                }
            }
        }

        private string WithGroups(int number)
        {
            var str = number.ToString();
            for (int i = str.Length - 3; i > 0; i -= 3)
            {
                str = str.Substring(0, i) + ',' + str.Substring(i);
            }
            return str;
        }
    }
}
