using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Structures;

namespace SP.Filters.Volume
{
    public class ChunkCut : ISoundFilter
    {
        private ISoundProvider _provider;
        private int _chunkSize;
        private int _totalSamples;
        private int _samplesSpeed;
        private int[] _lastTotalSamples;
        private Thread _updater;
        private Thread _requester;
        private SoundQueue _queue;
        private object _lock = new object();

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is int chunkSize)
            {
                _chunkSize = chunkSize;
            }
            else
            {
                throw new ArgumentException("Arg2: Int expected");
            }
            _queue = new SoundQueue();
            SetupMeasureThread();
        }

        public SoundData ReadPart(int length)
        {
            _totalSamples += length;
            SetupRequestThread();
            SoundData samples;
            var needMode = false;
            lock (_lock)
            {
                needMode = _queue.TotalLength < length;
            }
            if (needMode)
            {
                var newSamples = _provider.ReadPart(length - _queue.TotalLength);
                lock (_lock)
                {
                    _queue.AppendToEnd(newSamples);
                }
            }
            lock (_lock)
            {
                samples = _queue.ObtainFromBeginning(length);
            }
            return samples;
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException($"CHUNKCUT has no SET parameters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"CHUNKCUT has no GET parameters");
        }

        public void Destroy()
        {
            _updater.Interrupt();
            _requester?.Interrupt();
        }

        private void SetupMeasureThread()
        {
            _totalSamples = 0;
            _samplesSpeed = 0;
            _lastTotalSamples = new int[4];
            _updater = new Thread((e) => UpdateMetrics());
            _updater.Start();
        }

        private void SetupRequestThread()
        {
            if (_requester == null)
            {
                _requester = new Thread((e) => RequestCallback());
                _requester.Start();
            }
        }

        private void RequestCallback()
        {
            while(true)
            {
                var samples = _provider.ReadPart(_chunkSize);
                lock (_lock)
                {
                    _queue.AppendToEnd(samples);
                }
                try
                {
                    var sleepTime = Math.Min(1.0, (float)_chunkSize / Math.Max(1, _samplesSpeed));
                    sleepTime = Math.Max(1, 1000 * sleepTime);
                    Thread.Sleep((int)sleepTime);
                }
                catch
                {
                    break;
                }
            }
        }

        private void UpdateMetrics()
        {
            while (true)
            {
                for (int i = 0; i < _lastTotalSamples.Length - 1; i++)
                {
                    _lastTotalSamples[i] = _lastTotalSamples[i + 1];
                }
                _lastTotalSamples[_lastTotalSamples.Length - 1] = _totalSamples;
                var speed = 0;
                for (int i = 0; i < _lastTotalSamples.Length - 1; i++)
                {
                    speed += _lastTotalSamples[i + 1] - _lastTotalSamples[i];
                }
                _samplesSpeed = speed / (_lastTotalSamples.Length - 1);
                _samplesSpeed = (int)(_samplesSpeed * 0.75);
                try
                {
                    Thread.Sleep(1000);
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
