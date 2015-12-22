using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public class ServiceLocation : IServiceLocation
	{
		public static ServiceLocation Instance { get; } = new ServiceLocation();

		static ServiceLocation()
		{
			ServiceLocator.SetLocatorProvider( AmbientValues.Get<IServiceLocator> );
		}

		readonly IAmbientValueRepository repository;
		readonly IAmbientKeyLocator defaultLocator;
		readonly IFactory<IServiceLocator, IAmbientKey> factory;

		ServiceLocation() : this( AmbientValueRepository.Instance, AmbientKeyLocator.Instance, AmbientKeyFactory.Instance )
		{}

		protected ServiceLocation( IAmbientValueRepository repository, IAmbientKeyLocator defaultLocator, IFactory<IServiceLocator, IAmbientKey> factory )
		{
			this.repository = repository;
			this.defaultLocator = defaultLocator;
			this.factory = factory;
		}

		public void Assign( IServiceLocator locator )
		{
			if ( IsAvailable )
			{
				repository.Remove( Locator );
			}

			locator.With( context =>
			{
				var keyLocator = context.GetInstance<IAmbientKeyLocator>() ?? defaultLocator;
				var key = keyLocator.Locate( context ) ?? factory.Create( context );
				repository.Add( key, context );
			} );
		}

		public bool IsAvailable => ServiceLocator.IsLocationProviderSet && Locator != null;

		public IServiceLocator Locator => ServiceLocator.Current;
	}
}