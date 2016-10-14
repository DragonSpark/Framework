using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects.Alteration
{
	sealed class Constructor : AdapterConstructorSource<IAlteration>
	{
		public static IParameterizedSource<Type, Func<object, IAlteration>> Default { get; } = new Constructor().ToCache();
		Constructor() : base( typeof(IAlteration<>), typeof(Adapter<>) ) {}
	}
}