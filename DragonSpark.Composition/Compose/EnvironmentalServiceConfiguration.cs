namespace DragonSpark.Composition.Compose;

sealed class EnvironmentalServiceConfiguration : ServiceComponentLocator<IServiceConfiguration>
{
	public static EnvironmentalServiceConfiguration Default { get; } = new();

	EnvironmentalServiceConfiguration() {}
}