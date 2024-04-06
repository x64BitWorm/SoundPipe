using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Add
{
    public class Add : ISoundFilter
    {
        private ISoundProvider _provider1;
        private ISoundProvider _provider2;

        public void Initialize(object[] args)
        {
            _provider1 = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _provider2 = args[1] as ISoundProvider ?? throw new ArgumentException("Arg2: SoundProvider expected");
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samples1 = _provider1.ReadPart(length);
            var samples2 = _provider2.ReadPart(length);
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = samples1[i] + samples2[i];
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
    }
}
