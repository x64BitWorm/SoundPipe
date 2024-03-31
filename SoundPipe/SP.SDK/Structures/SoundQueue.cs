using SP.SDK.Primitives;

namespace SP.SDK.Structures
{
    public class SoundQueue
    {
        private readonly Queue<SoundData> _queue;
        private int _totalLength;

        public int TotalLength => _totalLength;

        public SoundQueue()
        {
            _queue = new Queue<SoundData>();
            _totalLength = 0;
        }

        public void AppendToEnd(SoundData data)
        {
            _queue.Enqueue(data);
            _totalLength += data.Length;
        }

        public SoundData ObtainFromBeginning(int length)
        {
            if (TotalLength < length)
            {
                throw new InvalidOperationException($"Not enough samples in queue ({TotalLength}, while {length} requested)");
            }
            var result = new SoundData(length);
            int remain = length;
            while (remain > 0)
            {
                var peek = _queue.Peek();
                if (peek.Length > remain)
                {
                    AppendToSoundData(result, peek, remain, length - remain);
                    RemoveBeginningFromArray(ref peek.samples, remain);
                    remain = 0;
                }
                else if (peek.Length < remain)
                {
                    AppendToSoundData(result, peek, peek.Length, length - remain);
                    _queue.Dequeue();
                    remain -= peek.Length;
                }
                else
                {
                    AppendToSoundData(result, peek, remain, length - remain);
                    _queue.Dequeue();
                    remain = 0;
                }
            }
            _totalLength -= length;
            return result;
        }

        private void AppendToSoundData(SoundData destination, SoundData source, int count, int from)
        {
            Array.ConstrainedCopy(source.samples, 0, destination.samples, from, count);
        }

        private void RemoveBeginningFromArray<T>(ref T[] source, int count)
        {
            var newSize = source.Length - count;
            Array.ConstrainedCopy(source, count, source, 0, newSize);
            Array.Resize(ref source, newSize);
        }
    }
}
