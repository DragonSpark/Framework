using DragonSpark.Extensions;
using System;

namespace DragonSpark.Runtime.Specifications
{
	public class TypeContextAwareSpecification : ContextAwareSpecificationBase<Type>
	{
		public TypeContextAwareSpecification( Type targetType ) : base( targetType )
		{}

		protected override bool IsSatisfiedByContext( Type context )
		{
			var result = base.IsSatisfiedByContext( context) && Context.Extend().IsAssignableFrom( context );
			return result;
		}
	}
}