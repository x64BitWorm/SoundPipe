using System.Diagnostics;
using System.Windows;

namespace SP.UI.Views
{
    public partial class DocumentationWindow : Window
    {
        public DocumentationWindow()
        {
            InitializeComponent();
        }

        private void HyperlinkRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var url = e.Uri.AbsoluteUri;
            if (!url.StartsWith("http"))
            {
                return;
            }
            var process = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            };
            Process.Start(process);
            e.Handled = true;
        }
    }
}
