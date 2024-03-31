using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
