using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class CommonRegistrations : IAlteration<IServiceCollection>
{
	public static CommonRegistrations Default { get; } = new();

	CommonRegistrations() : this(false) {}

	readonly Switch _configure;

	public CommonRegistrations(Switch configure) => _configure = configure;

	public IServiceCollection Get(IServiceCollection parameter)
		=> _configure.Up()
			   ? parameter.Start<Connect>().Include(x => x.Dependencies.Recursive()).Singleton().Then
			   : parameter;
}