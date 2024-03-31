using System.Windows;
using System.Windows.Controls;

namespace SP.UI.Components.PropertiesStore
{
    public partial class PropertiesStore
    {
        private void InputTypeTextChanged(object sender, TextChangedEventArgs e)
        {
            var element = sender as TextBox;
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType.Id, element.Text);
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType.Id, (float)e.NewValue);
        }

        private void ComboValueChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as ComboBox;
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType.Id, element.SelectedItem);
        }

        private PropertyType GetTypeFromView(object sender)
        {
            var element = sender as Control;
            var root = element.FindName("Root") as StackPanel;
            return root.Tag as PropertyType;
        }
    }
}
