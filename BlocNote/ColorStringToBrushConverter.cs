using System.Globalization;
using Microsoft.Maui.Graphics;

namespace Converter
{


public class ColorStringToBrushConverter : IValueConverter
{

    public static Color ConvertSystemDrawingColorToMauiColor(System.Drawing.Color systemColor)
    {
        float red = systemColor.R / 255f;
        float green = systemColor.G / 255f;
        float blue = systemColor.B / 255f;
        float alpha = systemColor.A / 255f;

        return new Color(red, green, blue, alpha);
    }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string colorString)
        {
            var color = System.Drawing.Color.FromName(colorString);
            var newcolor = ConvertSystemDrawingColorToMauiColor(color);
            return new SolidColorBrush(newcolor);
        } 

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}