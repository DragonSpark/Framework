using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Runtime.Initialization;


public sealed class PerformInitialization : IStopAware
{
    public static PerformInitialization Default { get; } = new();

    PerformInitialization()
        : this(Mobile.Runtime.Initialization.PerformInitialization.Default, Send<ApplicationInitializedMessage>.Default) {}
    
    readonly IStopAware                              _previous;
    readonly ICommand<ApplicationInitializedMessage> _send;

    public PerformInitialization(IStopAware previous, ICommand<ApplicationInitializedMessage> send)
    {
        _previous  = previous;
        _send = send;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        await _previous.Off(parameter);
        _send.Execute(new());
    }
}