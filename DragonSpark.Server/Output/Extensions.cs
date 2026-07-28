using DragonSpark.Model.Sequences;
using DragonSpark.Server.Output.Compose;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Output;

public static class Extensions
{
	public static IServiceCollection AddOutputCache(this IServiceCollection @this, params IOutputsPolicy[] parameter)
		=> @this.AddOutputCache(new ApplyPolicies(parameter).Execute);

	public static StartRegistration<T> Start<T>(this IOutputCacheStore @this, Array<IOutputKey> keys)
		where T : notnull => new(@this, keys);
}