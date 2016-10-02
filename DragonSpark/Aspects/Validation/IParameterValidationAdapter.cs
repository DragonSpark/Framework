using System.Reflection;
using DragonSpark.Specifications;

namespace DragonSpark.Aspects.Validation
{
	public interface IParameterValidationAdapter : ISpecification<MethodInfo>, ISpecification<object> {}
}