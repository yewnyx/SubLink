using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace tech.sublink.SubLinkEditor.UI;

/// <summary>
/// 
/// </summary>
internal class TypeToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        new SolidColorBrush(VariableTypeInspector.GetColorFromType(value as Type));

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 
/// </summary>
internal class TypeToLinearGradientConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        Color varColor = VariableTypeInspector.GetColorFromType(value as Type);
        varColor = Color.Multiply(varColor, 0.4f);
        varColor.A = 153;

        LinearGradientBrush linearBrush = new() {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1)
        };
        linearBrush.GradientStops.Add(new(varColor, 0));
        linearBrush.GradientStops.Add(new(Color.FromArgb(153, 0, 0, 0), 0.4));

        return linearBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
