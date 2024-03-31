using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.VolumeLevel
{
    public class VolumeLevel : ISoundFilter
    {
        private ISoundProvider _provider;
        private float _lastLevel;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _lastLevel = 0;
        }

        public SoundData ReadPart(int length)
        {
            var result = _provider.ReadPart(length);
            double accumulator = 0.0;
            foreach (var item in result.samples)
            {
                accumulator += Math.Abs(item.left) + Math.Abs(item.right);
            }
            _lastLevel = (float)(accumulator / (length * 2));
            return result;
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "level":
                    return _lastLevel;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'level' is supported");
            }
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException("There is no SET variables in VOLUMELEVEL filter");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
