using System;
using DragonSpark.Specifications;

namespace DragonSpark.Windows
{
	public sealed class IsExecutingInManagedHostSpecification : SpecificationBase<AppDomain>
	{
		public static IsExecutingInManagedHostSpecification Default { get; } = new IsExecutingInManagedHostSpecification();
		IsExecutingInManagedHostSpecification() {}

		public override bool IsSatisfiedBy( AppDomain parameter ) => parameter.DomainManager != null;
	}
}