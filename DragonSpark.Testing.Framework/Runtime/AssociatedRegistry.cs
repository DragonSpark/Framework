using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized.Caching;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Runtime
{
	public class AssociatedRegistry : Cache<IFixture, IServiceRepository>
	{
		public static AssociatedRegistry Default { get; } = new AssociatedRegistry();

		AssociatedRegistry() : base( instance => new FixtureRegistry( instance ) ) {}
	}
}