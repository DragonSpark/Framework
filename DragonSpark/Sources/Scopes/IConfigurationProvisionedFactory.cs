using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;

namespace DragonSpark.Sources.Scopes
{
	public interface IConfigurationProvisionedFactory<TConfiguration, out TResult> : IConfigurationProvisionedFactory<TConfiguration, object, TResult> {}
	public interface IConfigurationProvisionedFactory<TConfiguration, TParameter, out TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TConfiguration> Seed { get; }
		
		IParameterizedScope<TParameter, ImmutableArray<IAlteration<TConfiguration>>> Alterations { get; }
	}
}