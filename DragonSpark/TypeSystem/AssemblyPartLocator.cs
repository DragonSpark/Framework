using System;
using System.Collections.Immutable;
using System.Reflection;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyPartLocator : ParameterizedSourceBase<Assembly, ImmutableArray<Assembly>>
	{
		public static AssemblyPartLocator Default { get; } = new AssemblyPartLocator();
		AssemblyPartLocator() : this( AssemblyLoader.Default.Get, AssemblyHintProvider.Default.Get, AssemblyPartQueryProvider.Default.Get ) {}

		readonly Func<string, ImmutableArray<Assembly>> assemblySource;
		readonly Func<Assembly, string> hintSource;
		readonly Func<Assembly, string> querySource;

		AssemblyPartLocator( Func<string, ImmutableArray<Assembly>> assemblySource, Func<Assembly, string> hintSource, Func<Assembly, string> querySource )
		{
			this.assemblySource = assemblySource;
			this.hintSource = hintSource;
			this.querySource = querySource;
		}

		public override ImmutableArray<Assembly> Get( Assembly parameter )
		{
			var path = string.Format( querySource( parameter ), hintSource( parameter ) );
			var result = assemblySource( path );
			return result;
		}
	}
}