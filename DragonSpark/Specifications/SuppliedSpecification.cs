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

		public SuppliedSpecification( bool satisfied ) // : base( Where<T>.Always )
		{
			this.satisfied = satisfied;
		}

		public override bool IsSatisfiedBy( [Optional]T parameter ) => satisfied;
	}
}