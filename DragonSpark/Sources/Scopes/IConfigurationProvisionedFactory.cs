using System.Collections.Immutable;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Sources.Scopes
{
	public interface IConfigurationProvisionedFactory<TConfiguration, out TResult> : IConfigurationProvisionedFactory<TConfiguration, object, TResult> {}
	public interface IConfigurationProvisionedFactory<TConfiguration, TParameter, out TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TConfiguration> Seed { get; }
		
		IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> Configurators { get; }
	}
}