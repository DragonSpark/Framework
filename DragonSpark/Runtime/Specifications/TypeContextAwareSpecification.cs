using DragonSpark.Extensions;
using System;

namespace DragonSpark.Runtime.Specifications
{
	public class TypeContextAwareSpecification : ContextAwareSpecificationBase<Type>
	{
		public TypeContextAwareSpecification( Type targetType ) : base( targetType )
		{}

		protected override bool IsSatisfiedByParameter( Type parameter ) => base.IsSatisfiedByParameter( parameter ) && Context.Adapt().IsAssignableFrom( parameter );
	}
}