using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects
{
	public interface IValidatedTypeDefinition : ITypeDefinition
	{
		IMethodStore Validation { get; }
		IMethodStore Execution { get; }
	}
}