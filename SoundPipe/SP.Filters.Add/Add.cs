using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Add
{
    public class Add : ISoundFilter
    {
        private ISoundProvider _provider1;
        private ISoundProvider _provider2;
        private ModeEnum _mode;

        public void Initialize(object[] args)
        {
            _provider1 = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _provider2 = args[1] as ISoundProvider ?? throw new ArgumentException("Arg2: SoundProvider expected");
            if (args[2] is string mode)
            {
                _mode = ModeFromString(mode);
            }
            else
            {
                throw new ArgumentException("Arg3: String (mode) expected");
            }
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samples1 = _provider1.ReadPart(length);
            var samples2 = _provider2.ReadPart(length);
            switch (_mode)
            {
                case ModeEnum.Add:
                    Processors.ProcessAdd(result, samples1, samples2);
                    break;
                case ModeEnum.Div2:
                    Processors.ProcessDiv2(result, samples1, samples2);
                    break;
                case ModeEnum.Diff:
                    Processors.ProcessDiff(result, samples1, samples2);
                    break;
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException("ADD has no setters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException("ADD has no getters");
        }

        public void Destroy()
        {
            // Nothing to do
        }

        private ModeEnum ModeFromString(string mode)
        {
            return mode switch
            {
                "div2" => ModeEnum.Div2,
                "diff" => ModeEnum.Diff,
                _ => ModeEnum.Add,
            };
        }
    }
}
