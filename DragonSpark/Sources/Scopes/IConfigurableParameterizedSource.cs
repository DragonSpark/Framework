using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Sources.Scopes
{
	public interface IConfigurableParameterizedSource<TParameter, TResult> : IParameterizedSource<TParameter, TResult>
	{
		IParameterizedScope<TParameter, TResult> Configuration { get; }
	}
}