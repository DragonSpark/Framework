using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects.Coercion
{
	sealed class Constructor : AdapterConstructorSource<ICoercer>
	{
		public static IParameterizedSource<Type, Func<object, ICoercer>> Default { get; } = new Constructor().ToCache();
		Constructor() : base( typeof(IParameterizedSource<,>), typeof(CoercerAdapter<,>) ) {}
	}
}