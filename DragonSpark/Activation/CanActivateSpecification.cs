using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation
{
	public sealed class CanActivateSpecification : SpecificationBase<Type>
	{
		public static ISpecification<Type> Default { get; } = new CanActivateSpecification().ToCachedSpecification();
		CanActivateSpecification() {}

		public override bool IsSatisfiedBy( Type parameter )
		{
			var info = parameter.GetTypeInfo();
			var result = !info.IsGenericTypeDefinition && !info.ContainsGenericParameters && !info.IsInterface && !info.IsAbstract && InstanceConstructors.Default.Get( info ).Any();
			return result;
		}
	}
}