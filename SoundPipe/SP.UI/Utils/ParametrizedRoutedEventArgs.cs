using System.Windows;

namespace SP.UI.Utils
{
    public class ParametrizedRoutedEventArgs: RoutedEventArgs
    {
        public object Parameter { get; set; }

        public ParametrizedRoutedEventArgs(RoutedEvent routedEvent, object source, object parameter) 
            : base(routedEvent, source)
        {
            Parameter = parameter;
        }
    }
}
