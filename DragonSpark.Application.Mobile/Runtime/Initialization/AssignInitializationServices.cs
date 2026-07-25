using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class AssignInitializationServices : Command<IServiceProvider>
{
    public static AssignInitializationServices Default { get; } = new();

    AssignInitializationServices() : base(InitializationServices.Default) {}
}