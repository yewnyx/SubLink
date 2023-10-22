using System.ComponentModel;
using System.Windows;

namespace tech.sublink.SubLinkEditor;

internal class PropertyChangedBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName) {
        Application.Current.Dispatcher.BeginInvoke(() => {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        });
    }
}
