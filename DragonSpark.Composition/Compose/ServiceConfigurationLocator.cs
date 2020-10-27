namespace DragonSpark.Composition.Compose
{
	sealed class ServiceConfigurationLocator : ServiceComponentLocator<IServiceConfiguration>
	{
		public static ServiceConfigurationLocator Default { get; } = new ServiceConfigurationLocator();

		ServiceConfigurationLocator() {}
	}
}