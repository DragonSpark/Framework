using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class ServiceLifetimeBehavior : IServiceBehavior
	{
		readonly UnityServiceHostBaseExtension _serviceHostBaseExtension;

		public ServiceLifetimeBehavior( UnityServiceHostBaseExtension serviceHostBaseExtension )
		{
			_serviceHostBaseExtension = serviceHostBaseExtension;
		}

		void IServiceBehavior.Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}

		void IServiceBehavior.AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}

		void IServiceBehavior.ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			serviceHostBase.Extensions.Add( _serviceHostBaseExtension );
		}
	}
}