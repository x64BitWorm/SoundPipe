using SP.UI.ViewModels;
using System.Windows;

namespace SP.UI.Views
{
    public partial class InputDialog : Window
    {
        private readonly InputDialogViewModel _model;

        public InputDialog(string caption, string defaultValue)
        {
            _model = new InputDialogViewModel()
            {
                Caption = caption,
                CurrentValue = defaultValue,
                ResultValue = defaultValue
            };
            DataContext = _model;
            InitializeComponent();
        }

        public string GetResult()
        {
            return _model.ResultValue;
        }

        public static string ShowAndGetResult(string caption, string defaultValue = "")
        {
            var dialog = new InputDialog(caption, defaultValue);
            dialog.ShowDialog();
            return dialog.GetResult();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            _model.ResultValue = _model.CurrentValue;
            Close();
        }
    }
}
