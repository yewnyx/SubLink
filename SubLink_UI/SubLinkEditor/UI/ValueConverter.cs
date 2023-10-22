using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tech.sublink.SubLinkEditor.UI;

internal class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        null;
}
