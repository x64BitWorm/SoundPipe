using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain
{
    public class Logger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[info] {message}");
        }

        public void LogError(Exception ex, string message)
        {
            Console.WriteLine($"[err] {message} {ex.Message}");
        }
    }
}
