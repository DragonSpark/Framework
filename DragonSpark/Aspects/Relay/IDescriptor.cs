using System;
using System.Collections.Generic;
using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public interface IDescriptor : ITypeAware, IParameterizedSource<IAspect>, IParameterizedSource<Type, IEnumerable<AspectInstance>> {}
}