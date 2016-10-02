using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyTypesStore : FactoryCache<Assembly, IEnumerable<Type>>
	{
		readonly static Func<Type, bool> Specification = ApplicationTypeSpecification.Default.ToSpecificationDelegate();

		readonly Func<Assembly, IEnumerable<Type>> types;
		public AssemblyTypesStore( Func<Assembly, IEnumerable<Type>> types )
		{
			this.types = types;
		}

		protected override IEnumerable<Type> Create( Assembly parameter ) => types( parameter ).Where( Specification ).ToImmutableArray().AsEnumerable();
	}
}