using SP.SDK;
using SP.SDK.Primitives;
using NAudio.Wave;

namespace SP.Filters.Input
{
    internal class Input : ISoundFilter
    {
        private string _fileName;
        private FileStream _fileStream;
        private IWaveProvider _waveProvider;
        private byte[] _buffer;

        public void Initialize(object[] args)
        {
            _fileName = args[0] as string ?? throw new ArgumentException("Arg1: FileName expected");
            _fileStream = new FileStream(_fileName, FileMode.Open);
            InitializeProvider();
            _buffer = Array.Empty<byte>();
        }

        public SoundData ReadPart(int length)
        {
            var result = new SoundData(length);
            var arrayLen = length * 2 * 2;
            if (_buffer.Length < arrayLen)
            {
                _buffer = new byte[arrayLen];
            }
            _waveProvider.Read(_buffer, 0, arrayLen);
            for (var i = 0; i < length; i++)
            {
                result[i] = Sample.From16Bit(new Span<byte>(_buffer, i * 4, 4));
            }
            return result;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException("INPUT has no setters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException("INPUT has no getters");
        }

        public void Destroy()
        {
            _fileStream.Dispose();
        }

        private void InitializeProvider()
        {
            if (_fileName.EndsWith(".mp3"))
            {
                _waveProvider = new Mp3FileReader(_fileStream)
                    .ToSampleProvider().ToWaveProvider16();
            }
            else
            {
                _waveProvider = new RawSourceWaveStream(_fileStream, new WaveFormat());
            }
        }
    }
}
