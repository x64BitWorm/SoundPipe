using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Condition
{
    public class Condition : ISoundFilter
    {
        private ISoundProvider _providerCondition;
        private ISoundProvider _providerIf;
        private ISoundProvider _providerElse;

        public void Initialize(object[] args)
        {
            _providerCondition = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _providerIf = args[1] as ISoundProvider ?? throw new ArgumentException("Arg2: SoundProvider expected");
            _providerElse = args[2] as ISoundProvider ?? throw new ArgumentException("Arg3: SoundProvider expected");
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var samplesCondition = _providerCondition.ReadPart(length);
            var samplesIf = _providerIf.ReadPart(length);
            var samplesElse = _providerElse.ReadPart(length);
            var sampleOne = new Sample(1, 1);
            for (var i = 0; i < length; i++)
            {
                var condition = samplesCondition[i];
                var sampleIf = samplesIf[i];
                var sampleElse = samplesElse[i];
                result[i] = sampleIf * condition + sampleElse * (sampleOne - condition);
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException($"CONDITION filter has no SET parameters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"CONDITION filter has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }
    }
}
