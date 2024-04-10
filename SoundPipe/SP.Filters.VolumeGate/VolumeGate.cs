using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.VolumeGate
{
    public class VolumeGate : ISoundFilter
    {
        private ISoundProvider _provider;
        private float _signalVolume;
        private float _lastLevel;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is float volume)
            {
                _signalVolume = volume;
            }
            else
            {
                throw new ArgumentException("Arg2: Float expected");
            }
            _lastLevel = 0;
        }

        public SoundData ReadPart(int length)
        {
            var samples = _provider.ReadPart(length);
            double accumulator = 0.0;
            foreach (var item in samples.samples)
            {
                accumulator += Math.Abs(item.left) + Math.Abs(item.right);
            }
            _lastLevel = (float)(accumulator / (length * 2));
            var result = new SoundData(length);
            if (_lastLevel >= _signalVolume)
            {
                result.ForEachSample(sample => new Sample(1, 1));
            }
            return result;
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "level":
                    return _signalVolume;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'level' is supported");
            }
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "level":
                    if (value is float volume)
                    {
                        _signalVolume = volume;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'level'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'level' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
