using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Aspects
{
	public sealed class OfSourceType : OfTypeAttribute
	{
		public OfSourceType() : base( typeof(IParameterizedSource<,>), typeof(ISource<>), typeof(Func<>), typeof(Func<,>) ) {}
	}
}