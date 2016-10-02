using DragonSpark.Configuration;
using System.Composition.Convention;

namespace DragonSpark.Composition
{
	public sealed class ConventionBuilderFactory : ConfigurableFactoryBase<ConventionBuilder>
	{
		public static ConventionBuilderFactory Default { get; } = new ConventionBuilderFactory();
		ConventionBuilderFactory() : base( () => new ConventionBuilder(), ConventionConfiguration.Default ) {}
	}
}