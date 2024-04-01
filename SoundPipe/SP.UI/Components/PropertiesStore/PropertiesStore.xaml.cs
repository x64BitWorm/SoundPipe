using System.Windows;
using System.Windows.Controls;
using SP.UI.Utils;

namespace SP.UI.Components.PropertiesStore
{
    public partial class PropertiesStore : UserControl
    {
        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register(nameof(Properties), typeof(PropertyType[]), typeof(PropertiesStore),
                new PropertyMetadata(null, OnPropertiesChanged));

        public static readonly RoutedEvent PropertyChangeEvent =
            EventManager.RegisterRoutedEvent(nameof(PropertyChange), RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(PropertiesStore));

        public PropertyType[] Properties
        {
            get => (PropertyType[])GetValue(PropertiesProperty);
            set  {
                SetValue(PropertiesProperty, value);
            }
        }

        public event RoutedEventHandler PropertyChange
        {
            add => AddHandler(PropertyChangeEvent, value);
            remove => RemoveHandler(PropertyChangeEvent, value);
        }

        public PropertiesStore()
        {
            InitializeComponent();
        }

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (PropertiesStore)d;
            var value = e.NewValue as PropertyType[];
        }

        private void PropertyChangeInvoke(PropertyType type, object value)
        {
            if (type.InteractionType == PropertyInteractionType.ReadOnly)
            {
                return;
            }
            var parameter = new ChangeEventParameter(type.Id, value);
            var newEvent = new ParametrizedRoutedEventArgs(PropertyChangeEvent, this, parameter);
            RaiseEvent(newEvent);
        }
    }
}
