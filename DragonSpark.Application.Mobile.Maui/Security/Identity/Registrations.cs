using DragonSpark.Application.Communication.Http;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<ILogin>()
                 .Forward<Login>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 .Then.TryDecorate<IAccessTokenProvider, AccessTokenProvider>();
        //
    }
}