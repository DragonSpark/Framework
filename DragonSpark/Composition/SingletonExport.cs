using System;
using System.Collections.Immutable;
using System.Composition.Hosting.Core;
using System.Reflection;

namespace DragonSpark.Composition
{
	public struct SingletonExport
	{
		public SingletonExport( PropertyInfo location, ImmutableArray<CompositionContract> contracts, Func<object> factory )
		{
			Location = location;
			Contracts = contracts;
			Factory = factory;
		}

		public PropertyInfo Location { get; }
		public ImmutableArray<CompositionContract> Contracts { get; }
		public Func<object> Factory { get; }
	}
}