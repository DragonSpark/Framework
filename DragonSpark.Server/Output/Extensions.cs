using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Output;

public static class Extensions
{
	public static IServiceCollection AddOutputCache(this IServiceCollection @this, params IOutputsPolicy[] parameter)
		=> @this.AddOutputCache(new ApplyPolicies(parameter).Execute);
}