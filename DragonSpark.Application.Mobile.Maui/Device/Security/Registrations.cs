using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.TryDecorate<ICompleteLogin, SavedLoginAwareCompleteLogin>();
    }
}