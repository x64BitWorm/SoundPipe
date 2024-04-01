using System.ComponentModel;

namespace SP.UI.Components.PropertiesStore
{
    public abstract class PropertyType: INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public PropertyInteractionType InteractionType { get; set; }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public abstract void SetDefaultValue(object value);
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
