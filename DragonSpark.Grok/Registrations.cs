using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok;

sealed class Registrations : Commands<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() : base(Chat.Registrations.Default, Image.Registrations.Default) {}
}