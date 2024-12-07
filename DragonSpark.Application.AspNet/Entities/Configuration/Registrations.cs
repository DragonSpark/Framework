using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities.Configuration;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ISettingAccessor>()
		         .Forward<SettingAccessor>()
		         .Decorate<ExistAwareSettingAccessor>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton();
	}
}