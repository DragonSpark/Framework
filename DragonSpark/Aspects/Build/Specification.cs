using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Aspects.Build
{
	sealed class Specification : SpecificationWithContextBase<Type, ImmutableArray<TypeAdapter>>
	{
		public Specification( params Type[] types ) : base( types.Select( type => type.Adapt() ).ToImmutableArray() ) {}

		public override bool IsSatisfiedBy( Type parameter )
		{
			if ( !Context.IsAssignableFrom( parameter ) )
			{
				throw new InvalidOperationException( $"{parameter} does not implement any of the types defined in {GetType()}, which are: {string.Join( ", ", Context.Select( t => t.ReferenceType.FullName ) )}" );
			}
			return true;
		}
	}
}