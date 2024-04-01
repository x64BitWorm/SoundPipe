using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;

namespace SP.Filters.Peeker
{
    public class Peeker: ISoundFilter
    {
        private ISoundProvider _provider;
        private bool _overlap;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _overlap = false;
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samples = _provider.ReadPart(length);
            if (!_overlap)
            {
                result = samples;
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "peek":
                    if (value is ActionTypeValue type)
                    {
                        _overlap = type == ActionTypeValue.Down;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'peek'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'peek' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"PEEKER filter has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
