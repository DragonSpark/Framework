using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class Initialize : IInitialize
{
    readonly ICommand               _token;
    readonly ICommand<NSDictionary> _process;
    
    public Initialize(IProcessNotifications notifications) : this(InitializeToken.Default, notifications) {}

    public Initialize(ICommand token, ICommand<NSDictionary> process)
    {
        _token   = token;
        _process = process;
    }

    public void Execute(NSDictionary? parameter)
    {
        _token.Execute();
        if (parameter is not null )
        {
            _process.Execute(parameter);   
        }
    }
}