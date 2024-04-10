using SP.SDK.Primitives;

namespace SP.Filters.Add
{
    public class Processors
    {
        public static void ProcessAdd(SoundData result, SoundData samples1, SoundData samples2)
        {
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = samples1[i] + samples2[i];
            }
        }

        public static void ProcessDiv2(SoundData result, SoundData samples1, SoundData samples2)
        {
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = (samples1[i] + samples2[i]) * 0.5f;
            }
        }

        public static void ProcessDiff(SoundData result, SoundData samples1, SoundData samples2)
        {
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = samples1[i] - samples2[i];
            }
        }
    }
}
