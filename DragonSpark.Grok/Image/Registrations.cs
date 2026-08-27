using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok.Image;

sealed class Registrations : Commands<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() : base(Grok.Registrations.Default, LocalRegistrations.Default) {}
}