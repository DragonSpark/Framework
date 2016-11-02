using DragonSpark.Sources.Scopes;
using System.Composition.Convention;

namespace DragonSpark.Composition
{
	public sealed class ConventionBuilderFactory : ConfigurationProvisionedSourceBase<ConventionBuilder>
	{
		public static ConventionBuilderFactory Default { get; } = new ConventionBuilderFactory();
		ConventionBuilderFactory() : base( () => new ConventionBuilder(), ConventionConfiguration.Default ) {}
	}
}