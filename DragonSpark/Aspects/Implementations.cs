using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Aspects
{
	static class Implementations
	{
		public static Func<object, Array<Type>> Arguments { get; }
			= Start.A.Selection.Of.Any.By.Type.Select(AspectImplementationArguments.Default).Get;
	}
}