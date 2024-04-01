namespace SP.SDK.Primitives
{
    public struct SoundData
    {
        public int Length => samples.Length;
        public Sample[] samples;

        public SoundData(int length)
        {
            samples = new Sample[length];
        }

        public Sample this[int index]
        {
            get => samples[index];
            set => samples[index] = value;
        }

        public SoundData Clone()
        {
            var result = new SoundData(Length);
            Array.ConstrainedCopy(samples, 0, result.samples, 0, samples.Length);
            return result;
        }
    }
}
