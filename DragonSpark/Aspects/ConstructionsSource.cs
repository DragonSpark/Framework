using System;
using DragonSpark.Aspects.Build;
using DragonSpark.Aspects.Coercion;
using DragonSpark.Aspects.Relay;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Coercion;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using PostSharp.Reflection;

namespace DragonSpark.Aspects
{
	sealed class ConstructionsSource : ItemSource<ObjectConstruction>
	{
		readonly static ObjectConstruction Relay = ObjectConstructionFactory<ApplyRelayAttribute>.Default.Get();
		readonly static ObjectConstruction Auto = ObjectConstructionFactory<ApplyAutoValidationAttribute>.Default.Get();
		readonly static ObjectConstructionFactory<ApplyCoercerAttribute> Coercer = ObjectConstructionFactory<ApplyCoercerAttribute>.Default;
		readonly static ObjectConstructionFactory<ApplySpecificationAttribute> Specification = ObjectConstructionFactory<ApplySpecificationAttribute>.Default;

		public ConstructionsSource( [OfType( typeof(ICoercer<,>) )]Type coercerType, [OfType( typeof(ISpecification<>) )]Type specificationType )
			: base( 
				// new ApplyGeneralizedImplementationsAttribute(),
				Coercer.GetUsing( coercerType ),
				Relay,
				Auto,
				Specification.GetUsing( specificationType )
			) {}
	}
}