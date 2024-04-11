using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Skip
{
    public class Skip : ISoundFilter
    {
        private ISoundProvider _provider;
        private int _skip;
        private bool _skipRequested;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is string skip)
            {
                if (!int.TryParse(skip, out _skip))
                {
                    _skip = 0;
                }
            }
            else
            {
                throw new ArgumentException("Arg2: Int expected");
            }
            _skipRequested = true;
        }

        public SoundData ReadPart(int length)
        {
            if (_skipRequested)
            {
                _provider.ReadPart(_skip);
                _skipRequested = false;
            }
            var samples = _provider.ReadPart(length);
            return samples;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException($"SKIP filter has no SET parameters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"SKIP filter has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
