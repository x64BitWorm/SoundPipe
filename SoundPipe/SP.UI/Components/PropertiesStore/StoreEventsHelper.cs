using SP.SDK.Models;
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
            PropertyChangeInvoke(propertyType, element.Text);
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType, (float)e.NewValue);
        }

        private void IntSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType, (int)e.NewValue);
        }

        private void ComboValueChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as ComboBox;
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType, element.SelectedItem);
        }

        private void ActionValueChanged(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var element = sender as Button;
            var propertyType = GetTypeFromView(sender);
            PropertyChangeInvoke(propertyType, 
                e.ButtonState == System.Windows.Input.MouseButtonState.Pressed
                ? ActionTypeValue.Down
                : ActionTypeValue.Up);
        }
        
        private PropertyType GetTypeFromView(object sender)
        {
            var element = sender as Control;
            var root = element.FindName("Root") as StackPanel;
            return root.Tag as PropertyType;
        }
    }
}
