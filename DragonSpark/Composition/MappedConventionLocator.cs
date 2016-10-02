using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Composition
{
	sealed class MappedConventionLocator : AlterationBase<Type>
	{
		public static MappedConventionLocator Default { get; } = new MappedConventionLocator();
		MappedConventionLocator() : this( CanActivateSpecification.Default ) {}

		readonly ISpecification<Type> specification;

		MappedConventionLocator( ISpecification<Type> specification )
		{
			this.specification = specification;
		}

		public override Type Get( Type parameter )
		{
			var name = $"{parameter.Namespace}.{ConventionCandidateNames.Default.Get( parameter )}";
			var result = name != parameter.FullName ? Get( parameter, name ) : null;
			return result;
		}

		Type Get( Type parameter, string name )
		{
			var type = parameter.Assembly().GetType( name );
			var result = type != null && specification.IsSatisfiedBy( type ) ? type : null;
			return result;
		}
	}
}