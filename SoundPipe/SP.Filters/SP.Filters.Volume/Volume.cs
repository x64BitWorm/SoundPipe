using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Volume
{
    public class Volume : ISoundFilter
    {
        private ISoundProvider _provider;
        private float _volume;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is float volume)
            {
                _volume = volume;
            }
            else
            {
                throw new ArgumentException("Arg2: Float expected");
            }
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samples = _provider.ReadPart(length);
            for (var i = 0; i < samples.Length; i++)
            {
                var sample = samples[i];
                sample = new Sample(sample.left * _volume, sample.right * _volume);
                result[i] = sample;
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "level":
                    if (value is float volume)
                    {
                        _volume = volume;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'volume'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'volume' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "level":
                    return _volume;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'volume' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
