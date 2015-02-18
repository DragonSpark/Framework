using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Communication
{
	public class ServiceLocationServiceHostFactory<TConfiguration, TServiceHost> : ServiceHostFactory 
		where TConfiguration : ServiceLocatorFactory, new()
		where TServiceHost : ServiceHost
	{
		protected override ServiceHost CreateServiceHost( Type serviceType, Uri[] baseAddresses )
		{
			Log.Information( string.Format( "Creating Host for: {0}", serviceType.AssemblyQualifiedName ) );

			var locator = ServiceLocationRegistry.Instance.Retrieve<TConfiguration>();

			var result = Activator.CreateInstance( typeof(TServiceHost), serviceType, baseAddresses ).To<TServiceHost>();
			result.Description.Behaviors.Add( new ServiceLocationServiceBehavior( locator ) );

			return result;
		}
	}
}