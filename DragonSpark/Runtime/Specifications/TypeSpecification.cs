using System;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime.Specifications
{
	public class TypeSpecification : Specification<Type>
	{
		public TypeSpecification( Type targetType ) : base( targetType )
		{}

		protected override bool IsSatisfiedByContext( Type context )
		{
			var result = Context.Extend().IsAssignableFrom( context );
			return result;
		}
	}
}