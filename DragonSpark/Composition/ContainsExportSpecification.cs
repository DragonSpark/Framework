using DragonSpark.Specifications;
using System;

namespace DragonSpark.Composition
{
	public sealed class ContainsExportSpecification : DelegatedAssignedSpecification<Type, AppliedExport>
	{
		public static ISpecification<Type> Default { get; } = new ContainsExportSpecification().ToCachedSpecification();
		ContainsExportSpecification() : base( AppliedExportLocator.Default.Get ) {}
	}
}