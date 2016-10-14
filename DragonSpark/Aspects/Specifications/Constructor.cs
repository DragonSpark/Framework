using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects.Specifications
{
	sealed class Constructor : AdapterConstructorSource<ISpecification>
	{
		public static IParameterizedSource<Type, Func<object, ISpecification>> Default { get; } = new Constructor().ToCache();
		Constructor() : base( GenericSpecificationTypeDefinition.Default.DeclaringType, typeof(SpecificationAdapter<>) ) {}
	}
}