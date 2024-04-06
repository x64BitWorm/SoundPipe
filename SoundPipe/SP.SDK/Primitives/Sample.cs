namespace SP.SDK.Primitives
{
    public struct Sample
    {
        public float left;
        public float right;

        public Sample(float l, float r)
        {
            left = l;
            right = r;
        }

        public void To16Bit(Span<byte> pointer)
        {
            var l = (short)(left * 32768);
            var r = (short)(right * 32768);
            pointer[0] = (byte)(l & 255);
            pointer[1] = (byte)(l >> 8);
            pointer[2] = (byte)(r & 255);
            pointer[3] = (byte)(r >> 8);
        }

        public static Sample From16Bit(Span<byte> pointer)
        {
            return new Sample((short)(pointer[0] | pointer[1] << 8) / 32768.0f, (short)(pointer[2] | pointer[3] << 8) / 32768.0f);
        }

        public static Sample operator +(Sample sample1, Sample sample2)
        {
            return new Sample(sample1.left + sample2.left, sample1.right + sample2.right);
        }

        public static Sample operator -(Sample sample1, Sample sample2)
        {
            return new Sample(sample1.left - sample2.left, sample1.right - sample2.right);
        }

        public static Sample operator *(Sample sample, float scalar)
        {
            return new Sample(sample.left * scalar, sample.right * scalar);
        }
    }
}
