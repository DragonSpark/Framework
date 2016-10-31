using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class FileSystemAssemblySource : SuppliedDeferredSource<ImmutableArray<Assembly>>
	{
		public static ISource<ImmutableArray<Assembly>> Default { get; } = new FileSystemAssemblySource();
		FileSystemAssemblySource() : this( AppDomain.CurrentDomain ) {}

		public FileSystemAssemblySource( AppDomain domain ) : base( DomainAssemblySource.Default.WithParameter( domain ).Get ) {}
	}
}