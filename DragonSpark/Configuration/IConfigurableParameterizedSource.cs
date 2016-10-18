using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Configuration
{
	public interface IConfigurableParameterizedSource<TParameter, TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TResult> Configuration { get; }
	}
}