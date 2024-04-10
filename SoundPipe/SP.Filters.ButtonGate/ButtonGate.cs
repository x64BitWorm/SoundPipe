using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;

namespace SP.Filters.ButtonGate
{
    public class ButtonGate: ISoundFilter
    {
        private bool _overlap;

        public void Initialize(object[] args)
        {
            _overlap = false;
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            if (_overlap)
            {
                result.ForEachSample(sample => new Sample(1, 1));
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
            throw new InvalidOperationException($"BUTTONGATE filter has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
