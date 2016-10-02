using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class AssemblyLoader : ParameterizedSourceBase<string, ImmutableArray<Assembly>>
	{
		public static AssemblyLoader Default { get; } = new AssemblyLoader();
		AssemblyLoader() : this( Configuration.AssemblyPathLocator.Get, Configuration.AssemblyLoader.Get ) {}

		readonly Func<string, ImmutableArray<string>> locator;
		readonly Func<string, Assembly> loader;

		public AssemblyLoader( Func<string, ImmutableArray<string>> locator, Func<string, Assembly> loader )
		{
			this.locator = locator;
			this.loader = loader;
		}

		public override ImmutableArray<Assembly> Get( string parameter ) => locator( parameter ).Select( loader ).ToImmutableArray();
	}
}