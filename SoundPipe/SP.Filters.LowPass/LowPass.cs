using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.LowPass
{
    public class LowPass : ISoundFilter
    {
        private ISoundProvider _provider;
        private float _delay;
        private float _delayValue;
        private Sample _current;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is float delay)
            {
                _delay = delay;
                ConvertFromParameter();
            }
            else
            {
                throw new ArgumentException("Arg2: Float expected");
            }
            _current = new Sample(0, 0);
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samples = _provider.ReadPart(length);
            for (var i = 0; i < samples.Length; i++)
            {
                var sample = samples[i];
                _current = _current * _delayValue + sample * (1.0f - _delayValue);
                result[i] = _current;
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "delay":
                    if (value is float delay)
                    {
                        _delay = delay;
                        ConvertFromParameter();
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'delay'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'delay' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "delay":
                    return _delay;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'delay' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }

        private void ConvertFromParameter()
        {
            _delayValue = (float)Math.Pow(_delay, 1.0 / 8);
        }
    }
}
