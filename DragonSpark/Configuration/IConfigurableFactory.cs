using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;

namespace DragonSpark.Configuration
{
	public interface IConfigurableFactory<TConfiguration, out TResult> : IConfigurableFactory<TConfiguration, object, TResult> {}
	public interface IConfigurableFactory<TConfiguration, TParameter, out TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TConfiguration> Seed { get; }
		
		IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> Configurators { get; }
	}
}