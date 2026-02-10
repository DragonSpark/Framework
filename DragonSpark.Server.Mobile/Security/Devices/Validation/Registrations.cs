using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() : this(SchemeName.Default, Application.Security.Tokens.SchemeName.Default) {}

    readonly string _name, _previous;

    public Registrations(string name, string previous)
    {
        _name     = name;
        _previous = previous;
    }

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<DeviceValidationSettings>()
                 //
                 .Start<IAuthorizationHandler>()
                 .Forward<AttestedDeviceHandler>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IIsAttested>()
                 .Forward<IsAttested>()
                 .Decorate<MemoryAwareIsAttested>()
                 .Decorate<SettingsAwareIsAttested>()
                 .Singleton()
                 //
                 .Then.AddAuthorization(x => x.AddPolicy(_name,
                                                         y =>
                                                         {
                                                             y.RequireAuthenticatedUser();
                                                             y.AddAuthenticationSchemes(_previous);
                                                             y.Requirements.Add(new AttestedDeviceRequirement());
                                                         }));
    }
}