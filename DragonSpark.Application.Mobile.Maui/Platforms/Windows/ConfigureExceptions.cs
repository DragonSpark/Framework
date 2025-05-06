using System;
using DragonSpark.Application.Mobile.Maui.Diagnostics;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Windows;

/// <summary>
/// Attribution: https://github.com/dotnet/maui/discussions/653#discussioncomment-3434251
/// </summary>
sealed class ConfigureExceptions : IConfigureExceptions
{
    readonly IConfigureExceptions _previous;
    readonly IMutable<Exception?> _last;

    public ConfigureExceptions(IConfigureExceptions previous) : this(previous, new Variable<Exception>()) {}

    public ConfigureExceptions(IConfigureExceptions previous, IMutable<Exception?> last)
    {
        _previous = previous;
        _last     = last;
    }

    public void Execute(UnhandledExceptionEventHandler parameter)
    {
        _previous.Execute(parameter);

        // For WinUI 3:
        //
        // * Exceptions on background threads are caught by AppDomain.CurrentDomain.UnhandledException,
        //   not by Microsoft.UI.Xaml.Application.Current.UnhandledException
        //   See: https://github.com/microsoft/microsoft-ui-xaml/issues/5221
        //
        // * Exceptions caught by Microsoft.UI.Xaml.Application.Current.UnhandledException have details removed,
        //   but that can be worked around by saved by trapping first chance exceptions
        //   See: https://github.com/microsoft/microsoft-ui-xaml/issues/7160
        //

        AppDomain.CurrentDomain.FirstChanceException += (_, args) => _last.Execute(args.Exception);

        Microsoft.UI.Xaml.Application.Current.UnhandledException
            += (sender, args) =>
               {
                   var exception = args.Exception.StackTrace is null ? _last.Get() : args.Exception;
                   if (exception is not null)
                   {
                       parameter(sender, new(exception, true));
                   }
               };
    }
}