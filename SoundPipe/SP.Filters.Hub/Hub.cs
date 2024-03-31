using SP.SDK;
using SP.SDK.Primitives;
using SP.SDK.Structures;

namespace SP.Filters.Hub
{
    public class Hub: ISoundFilter
    {
        private ISoundProvider _provider;
        private int _outputs;
        private int _currentOutput;
        private SoundQueue[] _queues;
        private readonly object _readLock = new object();

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            _outputs = 2;
            _currentOutput = 0;
            _queues = Enumerable.Range(0, _outputs).Select(i => new SoundQueue()).ToArray();
        }

        public SoundData ReadPart(int length)
        {
            lock (_readLock)
            {
                var queue = _queues[_currentOutput];
                _currentOutput = (_currentOutput + 1) % _outputs;
                if (queue.TotalLength < length)
                {
                    var remain = length - queue.TotalLength;
                    var newData = _provider.ReadPart(remain);
                    foreach (var item in _queues)
                    {
                        item.AppendToEnd(newData);
                    }
                }
                return queue.ObtainFromBeginning(length);
            }
        }

        public void SetHotValue(string key, object value)
        {
            throw new InvalidOperationException("HUB has no setters");
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException("HUB has no getters");
        }

        public void Destroy()
        {
            // nothing to dispose
        }
    }
}
