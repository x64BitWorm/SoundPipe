using SP.SDK.Primitives;

namespace SP.Filters.Channel
{
    public class Processors
    {
        public static void ProcessLeft(SoundData chunk)
        {
            chunk.ForEachSample(s => new Sample(s.left, 0));
        }

        public static void ProcessRight(SoundData chunk)
        {
            chunk.ForEachSample(s => new Sample(0, s.right));
        }

        public static void ProcessSwap(SoundData chunk)
        {
            chunk.ForEachSample(s => new Sample(s.right, s.left));
        }

        public static void ProcessBetween(SoundData chunk)
        {
            chunk.ForEachSample(s =>
            {
                var middle = (s.left + s.right) / 2;
                return new Sample(middle, middle);
            });
        }
    }
}
