using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Runtime;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter) {}
}