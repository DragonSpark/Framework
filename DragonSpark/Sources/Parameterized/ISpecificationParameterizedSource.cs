using DragonSpark.Specifications;

namespace DragonSpark.Sources.Parameterized
{
	public interface ISpecificationParameterizedSource<in TParameter, out TResult> : IParameterizedSource<TParameter, TResult>, ISpecification<TParameter> {}
}