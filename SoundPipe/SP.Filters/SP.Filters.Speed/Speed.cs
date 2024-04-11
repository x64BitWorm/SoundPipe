using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Speed
{
    public class Speed : ISoundFilter
    {
        private ISoundProvider _provider;
        private float _speed;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is float speed)
            {
                _speed = speed;
            }
            else
            {
                throw new ArgumentException("Arg2: Float expected");
            }
        }

        public SoundData ReadPart(int length)
        {
            var speed = _speed;
            var result = new SoundData(length);
            var requested = (int)Math.Ceiling(length * speed);
            var data = _provider.ReadPart(requested);
            for (var i = 0; i < length - 1; i++)
            {
                var newIndexF = i * speed;
                var newIndexI = (int)newIndexF;
                var start = data[newIndexI];
                var end = data[(int)((i + 1) * speed)];
                var step = speed < 1.0 ? (newIndexF - newIndexI) : 0.0;
                result[i] = start + (end - start) * (float)step;
            }
            result[length - 1] = data[data.Length - 1];
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "speed":
                    if (value is float speed)
                    {
                        _speed = speed;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'speed'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'speed' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "speed":
                    return _speed;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'speed' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
