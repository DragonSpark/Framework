using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects.Specifications
{
	sealed class SpecificationConstructor : AdapterConstructorSource<ISpecification>
	{
		public static IParameterizedSource<Type, Func<object, ISpecification>> Default { get; } = new SpecificationConstructor().ToCache();
		SpecificationConstructor() : base( GenericSpecificationTypeDefinition.Default.DeclaringType, typeof(SpecificationAdapter<>) ) {}
	}
}