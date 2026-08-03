using System;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace DesktopApp.Util;

public class ImageFileConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path && File.Exists(path))
        {
            try
            {
                using (var mem = new MemoryStream(File.ReadAllBytes(path)))
                {
                    var bitmap = new Bitmap(mem);
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingNotification.UnsetValue;
    }
}