using SP.Domain;
using SP.Domain.Models;
using SP.UI.Models;
using System.IO;

namespace SP.UI.Services
{
    public class ShemeProvider
    {
        private readonly ShemeManager _shemeManager;

        public ShemeProvider(ShemeManager shemeManager)
        {
            _shemeManager = shemeManager;
        }

        public ShemeConstructor<UINodeInfo> ReadShemeFromFile(string fileName)
        {
            var shemeContent = File.ReadAllText(fileName);
            return _shemeManager.ReadShemeConstructor<UINodeInfo>(shemeContent);
        }

        public void WriteShemeToFile(ShemeConstructor<UINodeInfo> sheme, string fileName)
        {
            var shemeContent = _shemeManager.WriteShemeConstructor<UINodeInfo>(sheme);
            File.WriteAllText(fileName, shemeContent);
        }

        public ShemeConstructor<UINodeInfo> CreateEmptySheme()
        {
            return _shemeManager.ReadShemeConstructor<UINodeInfo>("[]");
        }
    }
}
