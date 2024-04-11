using NAudio.Wave;
using SP.SDK;
using SP.SDK.Primitives;

namespace SP.Filters.Speaker
{
    public class Speaker : ISoundFilter
    {
        private ISoundProvider _provider;
        private ProxyReader _proxyReader;
        private WaveOutEvent _waveOut;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _proxyReader = new ProxyReader(_provider);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_proxyReader);
        }

        public SoundData ReadPart(int length)
        {
            throw new InvalidOperationException("SPEAKER is 'consumer' filter");
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "state":
                    if (value is string state)
                    {
                        if (state == "play")
                        {
                            _waveOut.Play();
                        }
                        else if (state == "pause")
                        {
                            _waveOut.Pause();
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid value for 'state'");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'state'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'state' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            switch (key)
            {
                case "state":
                    return _waveOut.PlaybackState == PlaybackState.Playing
                        ? "play"
                        : "pause";
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'state' is supported");
            }
        }

        public void Destroy()
        {
            _waveOut.Stop();
            _waveOut.Dispose();
        }
    }
}
