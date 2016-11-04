using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyResourcePathSelector : ParameterizedSingletonScope<string, ImmutableArray<string>>
	{
		public static AssemblyResourcePathSelector Default { get; } = new AssemblyResourcePathSelector();
		AssemblyResourcePathSelector() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : ParameterizedSourceBase<string, ImmutableArray<string>>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override ImmutableArray<string> Get( string parameter ) => Items<string>.Immutable;
		}
	}
}