using DragonSpark.Specifications;
using System;

namespace DragonSpark.Windows.Setup
{
	public sealed class IsExecutingInManagedHostSpecification : SpecificationBase<AppDomain>
	{
		public static IsExecutingInManagedHostSpecification Default { get; } = new IsExecutingInManagedHostSpecification();
		IsExecutingInManagedHostSpecification() {}

		public override bool IsSatisfiedBy( AppDomain parameter ) => parameter.DomainManager != null;
	}
}