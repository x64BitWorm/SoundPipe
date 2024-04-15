using System;
using System.Linq;

namespace SP.UI.Services
{
    public class StartupArgumentsProvider
    {
        private string _shemePath;

        public StartupArgumentsProvider()
        {
            _shemePath = null;
        }

        public void InitializeFromArguments(string[] arguments)
        {
            _shemePath = arguments.FirstOrDefault(argument => argument.EndsWith(".spf"));
        }

        public string GetShemePath()
        {
            return _shemePath;
        }

        public bool ShemePathPassed()
        {
            return _shemePath != null;
        }
    }
}
