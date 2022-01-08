using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class Service<T> : ISelect<IServiceCollection, T> where T : notnull
{
	public static Service<T> Default { get; } = new();

	Service() {}

	public T Get(IServiceCollection parameter)
	{
		using var provider = parameter.BuildServiceProvider(false);
		var       next     = new ActivationAwareServiceProvider(provider);
		var       result   = new LocateAwareServiceProvider(next, parameter).GetRequiredService<T>();
		return result;
	}
}