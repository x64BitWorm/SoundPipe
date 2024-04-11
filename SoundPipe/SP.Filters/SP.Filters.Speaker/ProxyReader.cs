using NAudio.Wave;
using SP.SDK;

namespace SP.Filters.Speaker
{
    internal class ProxyReader : IWaveProvider
    {
        private readonly ISoundProvider _provider;
        private readonly WaveFormat _waveFormat;

        public WaveFormat WaveFormat => _waveFormat;

        public ProxyReader(ISoundProvider provider)
        {
            _provider = provider;
            _waveFormat = new WaveFormat(44100, 2);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var samples = count / 4;
            var data = _provider.ReadPart(samples);
            for (var i = 0; i < data.Length; i++)
            {
                data.samples[i].To16Bit(new Span<byte>(buffer, i * 4, 4));
            }
            return data.Length * 4;
        }
    }
}
