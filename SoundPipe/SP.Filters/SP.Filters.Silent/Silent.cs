using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Silent
{
    public class Silent : ISoundFilter
    {
        public void Initialize(object[] args)
        {
            // nothing to do
        }

        public SoundData ReadPart(int length)
        {
            return new SoundData(length);
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException($"SILENT has no SET parameters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"SILENT has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
