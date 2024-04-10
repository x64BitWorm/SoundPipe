using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.SinGenerator
{
    public class SinGenerator : ISoundFilter
    {
        private int _discretization;
        private int _frequency;
        private float _volume;
        private int _offset;

        public void Initialize(object[] args)
        {
            if (args[0] is string discretization)
            {
                if (!int.TryParse(discretization, out _discretization))
                {
                    _discretization = 44100;
                }
            }
            else
            {
                throw new ArgumentException("Arg1: Int expected");
            }
            if (args[1] is int frequency)
            {
                _frequency = frequency;
            }
            else
            {
                throw new ArgumentException("Arg2: Int expected");
            }
            if (args[2] is float volume)
            {
                _volume = volume;
            }
            else
            {
                throw new ArgumentException("Arg3: Float expected");
            }
            _offset = 0;
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            for (var i = 0; i < result.Length; i++)
            {
                var step = (float)(i + _offset) / _discretization * Math.PI * _frequency;
                var value = (float)Math.Sin(step) * _volume;
                result[i] = new Sample(value, value);
            }
            _offset += length;
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "frequency":
                    if (value is int frequency)
                    {
                        _frequency = frequency;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'frequency'");
                    }
                    break;
                case "volume":
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
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'frequency,volume' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "frequency":
                    return _frequency;
                case "volume":
                    return _volume;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'frequency,volume' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
