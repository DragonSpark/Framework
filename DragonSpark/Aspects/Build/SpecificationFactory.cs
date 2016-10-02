using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Aspects.Build
{
	sealed class SpecificationFactory : IParameterizedSource<IEnumerable<ITypeAware>, Func<Type, bool>>
	{
		public static SpecificationFactory Default { get; } = new SpecificationFactory();
		SpecificationFactory() {}

		public Func<Type, bool> Get( IEnumerable<ITypeAware> parameter ) => new Specification( parameter.Select( definition => definition.DeclaringType ).Distinct().ToArray() ).ToSpecificationDelegate();
	}
}