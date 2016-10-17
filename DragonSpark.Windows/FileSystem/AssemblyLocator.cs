using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class AssemblyLocator : QueryableResourceLocator
	{
		public static IParameterizedSource<string, ImmutableArray<string>> Default { get; } = new AssemblyLocator().ToEqualityCache();
		AssemblyLocator() : base( IsAssemblyFileSpecification.Default.IsSatisfiedBy ) {}
	}
}
