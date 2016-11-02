using DragonSpark.Application;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyTypesStore : CacheWithImplementedFactoryBase<Assembly, ImmutableArray<Type>>
	{
		readonly static Func<Type, bool> Specification = ApplicationTypeSpecification.Default.ToSpecificationDelegate();

		readonly Func<Assembly, IEnumerable<Type>> types;
		public AssemblyTypesStore( Func<Assembly, IEnumerable<Type>> types )
		{
			this.types = types;
		}

		protected override ImmutableArray<Type> Create( Assembly parameter ) => types( parameter ).Where( Specification ).ToImmutableArray();
	}
}