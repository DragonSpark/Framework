using Android.Content;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class Initialize : IInitialize
{
    readonly ICommand         _token;
    readonly ICommand<Intent> _process;

    public Initialize(IProcessNotifications notifications) : this(InitializeToken.Default, notifications) {}

    public Initialize(ICommand token, ICommand<Intent> process)
    {
        _token   = token;
        _process = process;
    }

    public void Execute(Intent parameter)
    {
        _token.Execute();
        _process.Execute(parameter);
    }
}