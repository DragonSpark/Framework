using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public class ServiceLocation : IServiceLocation
	{
		public static ServiceLocation Instance { get; } = new ServiceLocation();

		ServiceLocation()
		{
			ServiceLocator.SetLocatorProvider( AmbientValues.Get<IServiceLocator> );
		}

		public void Assign( IServiceLocator locator )
		{
			if ( IsAvailable )
			{
				AmbientValues.Remove( Locator );
			}

			locator.With( context =>
			{
				var keyLocator = context.GetInstance<IAmbientKeyLocator>() ?? AmbientKeyLocator.Instance;
				var key = keyLocator.Locate( context ) ?? new AmbientKey<IServiceLocator>( EqualsOrNull( context ) );
				AmbientValues.Register( key, context );
			} );
		}

		static ISpecification EqualsOrNull( IServiceLocator context )
		{
			var items = new[] { context, null }.Select( item => new EqualityContextAwareSpecification( item ) ).Cast<ISpecification>();
			var result = new AnySpecification( items.ToArray() );
			return result;
		}

		public bool IsAvailable => ServiceLocator.IsLocationProviderSet && Locator != null;

		public IServiceLocator Locator => ServiceLocator.Current;
	}
}