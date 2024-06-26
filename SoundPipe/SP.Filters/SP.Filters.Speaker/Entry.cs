﻿using SP.SDK;
using SP.SDK.Models;
using SP.SDK.Primitives;
using SP.SDK.Primitives.Types;

namespace SP.Filters.Speaker
{
    public class Entry : IFilterEntry
    {
        public ISoundFilter ConstructFilter()
        {
            return new Speaker();
        }

        public DynamicParameter[] ConstructorParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("input", "Входной воспроизводимый поток", new DynamicStreamType())
            };
        }

        public string FilterDescription()
        {
            return "Модуль вывода звукового потока на динамик устройства (дискретизация - 44100)";
        }

        public string FilterName()
        {
            return "Speaker";
        }

        public int FilterVersion()
        {
            return 1;
        }

        public FilterType GetFilterType()
        {
            return FilterType.Consumer;
        }

        public FilterGroupType GetGroupType()
        {
            return FilterGroupType.Consumer;
        }

        public Uri GetSourceCodeLink()
        {
            return new Uri("https://github.com/x64BitWorm/SoundPipe/tree/main/SoundPipe/SP.Filters.Speaker");
        }

        public DynamicParameter[] HotParamaters()
        {
            return new DynamicParameter[]
            {
                new DynamicParameter("state", "Статус воспроизведения", new DynamicEnumType(new[] { "play", "pause" }))
            };
        }
    }
}
