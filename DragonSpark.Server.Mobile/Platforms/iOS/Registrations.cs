using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public void Execute(IServiceCollection parameter) {}
}