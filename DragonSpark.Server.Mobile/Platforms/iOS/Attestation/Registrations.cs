using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : class, IAttestationRecord
{
    public static Registrations<T> Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IAttestationRecord<T>>()
                 .Forward<AttestationRecord<T>>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton();
    }
}