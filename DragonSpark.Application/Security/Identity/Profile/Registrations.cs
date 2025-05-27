using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Profile;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton<ICurrentProfile, CurrentProfile>().AddSingleton<ICreateProfile, DefaultCreateProfile>();
	}
}