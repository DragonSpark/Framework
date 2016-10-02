using System;
using DragonSpark.Aspects.Build;
using DragonSpark.Specifications;

namespace DragonSpark.Aspects
{
	public abstract class SpecificationTypeDefinitionBase : TypeDefinitionWithPrimaryMethodBase
	{
		protected SpecificationTypeDefinitionBase( Type specificationType ) : base( new MethodStore( specificationType, nameof(ISpecification<object>.IsSatisfiedBy) ) ) {}
	}
}