using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain
{
    public class Settings
    {
        public string FiltersPath { get; private set; }

        public Settings()
        {
            FiltersPath = "C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\Filters\\";
        }
    }
}
