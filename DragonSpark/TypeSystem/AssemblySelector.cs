using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblySelector : ParameterizedSourceBase<string, ImmutableArray<Assembly>>
	{
		public static AssemblySelector Default { get; } = new AssemblySelector();
		AssemblySelector() : this( AssemblyResourcePathSelector.Default.Get, AssemblyLoader.Default.Get ) {}

		readonly Func<string, ImmutableArray<string>> locator;
		readonly Func<string, Assembly> loader;
		
		[UsedImplicitly]
		public AssemblySelector( Func<string, ImmutableArray<string>> locator, Func<string, Assembly> loader )
		{
			this.locator = locator;
			this.loader = loader;
		}

		public override ImmutableArray<Assembly> Get( string parameter ) => locator( parameter ).Select( loader ).ToImmutableArray();
	}

	public sealed class AssemblyResourcePathSelector : ConfigurableParameterizedSource<string, ImmutableArray<string>>
	{
		public static AssemblyResourcePathSelector Default { get; } = new AssemblyResourcePathSelector();
		AssemblyResourcePathSelector() : base( path => Items<string>.Immutable ) {}
	}

	public sealed class AssemblyLoader : ConfigurableParameterizedSource<string, Assembly>
	{
		public static AssemblyLoader Default { get; } = new AssemblyLoader();
		AssemblyLoader() {}
	}
}