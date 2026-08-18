using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Server.Requests.Warmup;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server;

sealed class CoreApplicationConfiguration : IAlteration<IApplicationBuilder>
{
	public static CoreApplicationConfiguration Default { get; } = new();

	CoreApplicationConfiguration() : this(x => x) {}

	readonly Func<IApplicationBuilder, IApplicationBuilder> _configure;

	public CoreApplicationConfiguration(Func<IApplicationBuilder, IApplicationBuilder> configure)
		=> _configure = configure;

	public IApplicationBuilder Get(IApplicationBuilder parameter)
		=> _configure(parameter.UseWarmupAwareHttpsRedirection().UseRouting()).UseAuthentication().UseAuthorization();
}