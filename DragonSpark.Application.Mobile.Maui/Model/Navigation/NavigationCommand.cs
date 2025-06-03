using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Model.Navigation;

public sealed class NavigationCommand : AsynchronousCommand<ShellNavigationState>
{
    public static NavigationCommand Default { get; } = new();

    NavigationCommand() : this(Shell.Current) {}

    public NavigationCommand(Shell subject) : base(subject.GoToAsync, Converter.Instance) {}

    sealed class Converter : TypeConverter
    {
        public static Converter Instance { get; } = new();

        Converter() {}

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => false;

        // ReSharper disable once TooManyArguments
        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? cultureInfo, object? value,
                                         Type destinationType) => throw new NotSupportedException();

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string) || sourceType == typeof(Uri);

        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            => value switch
            {
                string str => (ShellNavigationState)str,
                Uri uri => (ShellNavigationState)uri,
                _ => throw new NotSupportedException(),
            };
    }
}