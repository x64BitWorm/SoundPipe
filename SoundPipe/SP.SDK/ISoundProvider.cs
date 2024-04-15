using SP.SDK.Primitives;

namespace SP.SDK
{
    public interface ISoundProvider
    {
        public SoundData ReadPart(int length);
    }
}
