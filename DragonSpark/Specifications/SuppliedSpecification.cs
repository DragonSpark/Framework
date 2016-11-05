using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Specifications
{
	public class SuppliedSpecification : SuppliedSpecification<object>
	{
		public SuppliedSpecification( bool satisfied ) : base( satisfied ) {}
	}

	public class SuppliedSpecification<T> : SpecificationBase<T>
	{
		readonly bool satisfied;

		public SuppliedSpecification( bool satisfied )
		{
			this.satisfied = satisfied;
		}

		public override bool IsSatisfiedBy( [Optional]T parameter ) => satisfied;
	}

	public sealed class SpecificationAdapter<T> : SpecificationBase<T>
	{
		readonly Func<bool> factory;

		public SpecificationAdapter( Func<bool> factory )
		{
			this.factory = factory;
		}

		public override bool IsSatisfiedBy( T parameter ) => factory();
	}
}