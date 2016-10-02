using DragonSpark.Extensions;
using System.Runtime.InteropServices;

namespace DragonSpark.Specifications
{
	public sealed class AssignedSpecification<T> : SpecificationBase<T>
	{
		public static ISpecification<T> Default { get; } = new AssignedSpecification<T>();
		AssignedSpecification() {}
	
		public override bool IsSatisfiedBy( [Optional]T parameter ) => parameter.IsAssigned();
	}
}