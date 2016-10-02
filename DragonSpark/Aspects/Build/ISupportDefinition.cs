using System;
using System.Collections.Generic;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using PostSharp.Aspects;

namespace DragonSpark.Aspects.Build
{
	public interface ISupportDefinition : ISpecification<Type>, IParameterizedSource<Type, IEnumerable<AspectInstance>> {}
}