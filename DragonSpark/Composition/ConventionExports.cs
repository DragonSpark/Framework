using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition
{
	public sealed class ConventionExports : ExportSourceBase<bool>
	{
		readonly ISpecification<Type> specification;

		public ConventionExports( IEnumerable<Type> interfaces, IEnumerable<Type> types ) : this( new ContainsItemSpecification<Type>( interfaces ), types ) {}

		ConventionExports( ISpecification<Type> specification, IEnumerable<Type> types ) : base( types, new DelegatedParameterizedSource<Type, bool>( specification.IsSatisfiedBy ) )
		{
			this.specification = specification;
		}

		protected override bool Validate( Type parameter ) => base.Validate( parameter ) || specification.IsSatisfiedBy( parameter );
	}
}