using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.SDK.Primitives;

namespace SP.SDK
{
    public interface ISoundProvider
    {
        public SoundData ReadPart(int length);
    }
}
