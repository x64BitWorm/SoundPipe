using Microsoft.Extensions.DependencyInjection;
using SP.UI.Views;
using System.Windows.Forms;

namespace SP.UI.ViewModels
{
    partial class MainViewModel
    {
        public void ShowDocumentation()
        {
            _serviceProvider.GetService<DocumentationWindow>()?.Show();
        }

        public void ShowAbout()
        {
            MessageBox.Show("SoundPipe - интерактивный инструмент обработки звукового графа\n\n"+
                "По работе с приложением читайте в документации, о подробностях устройства приложения и процессе создания собственных фильтров читайте на странице с исходным кодом\n\n"+
                "x64BitWorm, 2024", "О приложении", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowSourceCode()
        {
            var url = "https://github.com/x64BitWorm/SoundPipe";
            var process = new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(process);
        }
    }
}
