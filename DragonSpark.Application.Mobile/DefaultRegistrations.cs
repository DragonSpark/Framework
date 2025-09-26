using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile;

public sealed class DefaultRegistrations : Commands<IServiceCollection>
{
    public static DefaultRegistrations Default { get; } = new();

    DefaultRegistrations() : base(Registrations.Default, Runtime.Registrations.Default) {}
}