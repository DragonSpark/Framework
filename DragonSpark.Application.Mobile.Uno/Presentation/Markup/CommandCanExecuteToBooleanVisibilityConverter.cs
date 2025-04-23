using System;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

public sealed class CommandCanExecuteToBooleanVisibilityConverter : IValueConverter
{
    // ReSharper disable once TooManyArguments
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var canExecute = value is ICommand c && c.CanExecute(parameter);
        return canExecute ? Visibility.Visible : Visibility.Collapsed;
    }

    // ReSharper disable once TooManyArguments
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}