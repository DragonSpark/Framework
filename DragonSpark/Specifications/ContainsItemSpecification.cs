using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Specifications
{
	public sealed class ContainsItemSpecification<T> : SpecificationBase<T>
	{
		readonly ImmutableArray<T> items;

		public ContainsItemSpecification( IEnumerable<T> items ) : this( items.ToImmutableArray() ) {}

		public ContainsItemSpecification( ImmutableArray<T> items )
		{
			this.items = items;
		}

		public override bool IsSatisfiedBy( T parameter ) => items.Contains( parameter );
	}
}