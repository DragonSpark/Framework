using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Activation
{
	public sealed class IsPublicTypeSpecification : SpecificationBase<Type>
	{
		public static IsPublicTypeSpecification Default { get; } = new IsPublicTypeSpecification();
		IsPublicTypeSpecification() {}

		public override bool IsSatisfiedBy( Type parameter )
		{
			var info = parameter.GetTypeInfo();
			var result = info.IsPublic || info.IsNestedPublic;
			return result;
		}
	}
}