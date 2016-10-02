using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects;
using System;

namespace DragonSpark.Aspects.Build
{
	public interface IAspectInstanceLocator : IParameterizedSource<Type, AspectInstance> {}
}