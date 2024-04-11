using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Channel
{
    public class Channel : ISoundFilter
    {
        private ISoundProvider _provider;
        private ModeEnum _mode;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is string mode)
            {
                _mode = ModeFromString(mode);
            }
            else
            {
                throw new ArgumentException("Arg2: String (mode) expected");
            }
        }

        public SoundData ReadPart(int length)
        {
            var samples = _provider.ReadPart(length);
            switch (_mode)
            {
                case ModeEnum.Left:
                    Processors.ProcessLeft(samples);
                    break;
                case ModeEnum.Right:
                    Processors.ProcessRight(samples);
                    break;
                case ModeEnum.Swap:
                    Processors.ProcessSwap(samples);
                    break;
                case ModeEnum.Between:
                    Processors.ProcessBetween(samples);
                    break;
            }
            return samples;
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "mode":
                    if (value is string mode)
                    {
                        _mode = ModeFromString(mode);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'mode'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'mode' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "mode":
                    return _mode.ToString().ToLower();
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'mode' is supported");
            }
        }

        public void Destroy()
        {
            // Nothing to do
        }

        private ModeEnum ModeFromString(string mode)
        {
            return mode switch
            {
                "left" => ModeEnum.Left,
                "right" => ModeEnum.Right,
                "swap" => ModeEnum.Swap,
                "between" => ModeEnum.Between,
                _ => ModeEnum.Normal,
            };
        }
    }
}
