using NAudio.Wave;
using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using System.Runtime.InteropServices;

namespace SP.Filters.SaveToFile
{
    public class SaveToFile : ISoundFilter
    {
        private ISoundProvider _provider;
        private string _fileName;
        private int _duration;

        public void Initialize(object[] args)
        {
            _provider = args[0] as ISoundProvider ?? throw new ArgumentException("Arg1: SoundProvider expected");
            if (args[1] is string fileName)
            {
                _fileName = fileName;
            }
            else
            {
                throw new ArgumentException("Arg2: String expected");
            }
            if (args[2] is int duration)
            {
                _duration = duration;
            }
            else
            {
                throw new ArgumentException("Arg3: Int expected");
            }
        }

        public SoundData ReadPart(int length)
        {
            throw new InvalidOperationException("SAVETOFILE is 'consumer' filter");
        }

        public void SetHotValue(string key, object value)
        {
            switch (key)
            {
                case "save":
                    if (value is ActionTypeValue type)
                    {
                        if (type == ActionTypeValue.Up)
                        {
                            new Thread((e) => FileSaver()).Start();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid type for 'save'");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Invalid parameter '{key}'. Only 'save' is supported");
            }
        }

        public object GetHotValue(string key)
        {
            throw new InvalidOperationException($"SAVETOFILE has no GET parameters");
        }

        public void Destroy()
        {
            // Nothing to do
        }

        private void FileSaver()
        {
            while (File.Exists(_fileName))
            {
                var path = Path.GetDirectoryName(_fileName);
                var name = Path.GetFileNameWithoutExtension(_fileName);
                var extension = Path.GetExtension(_fileName);
                _fileName = path + name + "1" + extension;
            }
            var soundData = _provider.ReadPart(_duration * 44100);
            var bytes = new byte[soundData.Length * 4];
            for (int i = 0; i < soundData.Length; i++)
            {
                var offset = i * 4;
                soundData[i].To16Bit(new Span<byte>(bytes, offset, bytes.Length - offset));
            }
            var buffer = new MemoryStream();
            buffer.Write(bytes);
            buffer.Position = 0;
            using (var wavStream = new RawSourceWaveStream(buffer, new WaveFormat(44100, 2)))
            {
                WaveFileWriter.CreateWaveFile(_fileName, wavStream);
            }
            MessageBox(IntPtr.Zero, $"Сохранено {soundData.Length} семплов в файл '{_fileName}'", "Сохранение файла", 0);
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr h, string m, string c, int type);
    }
}
