namespace DragonSpark.Objects
{
	/*public class CacheManagerCreator : IObjectCreationPolicy
	{
		readonly CacheManagerFactoryHelper helper = new CacheManagerFactoryHelper();

		public CacheManagerCreator()
		{
			MaximumElementsInCacheBeforeScavenging = 1000;
			NumberToRemoveWhenScavenging = 10;
			ExpirationPollFrequencyInSeconds = 60;
			BackingStore = new NamedTypeBuildKey { Type = typeof(NullBackingStore) };
			CachingInstrumentationProvider = new NamedTypeBuildKey { Type = typeof(CachingInstrumentationProvider) };
		}

		public int MaximumElementsInCacheBeforeScavenging { get; set; }

		public int NumberToRemoveWhenScavenging { get; set; }

		public int ExpirationPollFrequencyInSeconds { get; set; }
		
		public NamedTypeBuildKey BackingStore { get; set; }
		
		public NamedTypeBuildKey CachingInstrumentationProvider { get; set; }

		object IObjectCreationPolicy.Create( IBuilderContext context )
		{
			var store = BackingStore.Create<IBackingStore>( context );
			var provider = CachingInstrumentationProvider.Create<CachingInstrumentationProvider>( context );
			var result = helper.BuildCacheManager( string.Empty, store, MaximumElementsInCacheBeforeScavenging, NumberToRemoveWhenScavenging, ExpirationPollFrequencyInSeconds, provider );
			return result;
		}
	}*/
}