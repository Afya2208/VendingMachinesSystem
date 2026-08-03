using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DesktopApp.Util;

public class DateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateOnly date)
        {
            return date.ToDateTime(TimeOnly.MinValue);
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        } 
        return null;
    }
}