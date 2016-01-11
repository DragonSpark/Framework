using DragonSpark.Extensions;
using System;

namespace DragonSpark.Runtime.Specifications
{
	public class TypeAssignableSpecification<T> : TypeAssignableSpecification
	{
		public static TypeAssignableSpecification<T> Instance { get; } = new TypeAssignableSpecification<T>();

		public TypeAssignableSpecification() : base( typeof(T) )
		{}
	}

	public class TypeAssignableSpecification : ContextAwareSpecificationBase<Type>
	{
		public TypeAssignableSpecification( Type targetType ) : base( targetType )
		{}

		protected override bool IsSatisfiedByParameter( Type parameter ) => base.IsSatisfiedByParameter( parameter ) && Context.Adapt().IsAssignableFrom( parameter );
	}
}