using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Specifications
{
	public abstract class AdapterSpecificationBase : SpecificationBase<Type>
	{
		protected AdapterSpecificationBase( params Type[] types ) : this( types.Select( type => type.Adapt() ).ToImmutableArray() ) {}

		AdapterSpecificationBase( ImmutableArray<TypeAdapter> adapters )
		{
			Adapters = adapters;
		}

		protected ImmutableArray<TypeAdapter> Adapters { get; }

		public override bool IsSatisfiedBy( Type parameter ) => Adapters.IsAssignableFrom( parameter );
	}
}