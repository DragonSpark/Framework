using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

public sealed class InverseBooleanToVisibilityConverter : IValueConverter
{
    // ReSharper disable once TooManyArguments
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    // ReSharper disable once TooManyArguments
    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => (Visibility)value == Visibility.Collapsed;
}