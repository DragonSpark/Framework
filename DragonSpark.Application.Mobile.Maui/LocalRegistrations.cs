using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IInitializeApplication>()
                 .Forward<DefaultInitializeApplication>()
                 .Singleton();

        // TODO: Implement
        /*MauiExceptions.UnhandledException += (_, args) =>
                                             {
                                                 if (args.ExceptionObject is Exception e)
                                                 {
                                                     _error(e);
                                                 }
                                             };*/
    }
}

/*/// <summary>
/// Attribution: https://github.com/dotnet/maui/discussions/653#discussioncomment-3434251
/// </summary>
public static class MauiExceptions
{
#if WINDOWS
   static Exception? _lastFirstChanceException;
#endif

    // We'll route all unhandled exceptions through this one event.
    public static event UnhandledExceptionEventHandler UnhandledException = delegate {};

    static MauiExceptions()
    {
        // This is the normal event expected, and should still be used.
        // It will fire for exceptions from iOS and Mac Catalyst,
        // and for exceptions on background threads from WinUI 3.

        AppDomain.CurrentDomain.UnhandledException += (sender, args) => { UnhandledException(sender, args); };

#if IOS || MACCATALYST
        // For iOS and Mac Catalyst
        // Exceptions will flow through AppDomain.CurrentDomain.UnhandledException,
        // but we need to set UnwindNativeCode to get it to work correctly. 
        // 
        // See: https://github.com/xamarin/xamarin-macios/issues/15252
        
        ObjCRuntime.Runtime.MarshalManagedException += (_, args) =>
        {
            args.ExceptionMode = ObjCRuntime.MarshalManagedExceptionMode.UnwindNativeCode;
        };

#elif ANDROID

        // For Android:
        // All exceptions will flow through Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser,
        // and NOT through AppDomain.CurrentDomain.UnhandledException

        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser
            += (sender, args) =>
               {
                   UnhandledException(sender!, new(args.Exception, true));
               };

#elif WINDOWS
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

        AppDomain.CurrentDomain.FirstChanceException += (_, args) =>
        {
            _lastFirstChanceException = args.Exception;
        };

        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, args) =>
        {
            var exception = args.Exception;

            if (exception.StackTrace is null)
            {
                exception = _lastFirstChanceException;
            }

            if (exception is not null)
            {
                UnhandledException(sender, new(exception, true));   
            }
        };
#endif
    }
}*/