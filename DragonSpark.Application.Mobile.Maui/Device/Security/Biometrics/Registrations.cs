using DragonSpark.Composition;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Biometrics;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IRequestUserBiometric>()
                 .Forward<RequestUserBiometric>()
                 .Include(x => x.Dependencies)
                 .Singleton()
            ;
    }
}