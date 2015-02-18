using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Server.Legacy.Communication
{
	public class ServiceLocationServiceBehavior : IServiceBehavior
	{
		readonly IServiceLocator locator;

		public ServiceLocationServiceBehavior( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			var interfaces = serviceDescription.ServiceType.GetInterfaces();
			serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany( cd => cd.Endpoints ).Apply( x =>
			{
				var type = interfaces./*Where( y => y.Assembly != typeof(IRequestHandler).Assembly ).*/FirstOrDefault( y => y.FromMetadata<ServiceContractAttribute, string>( z => x.ContractNamespace ??  "http://tempuri.org/" ) == x.ContractNamespace && y.Name == x.ContractName ) ?? serviceDescription.ServiceType;
				x.DispatchRuntime.InstanceProvider = new ServiceLocationInstanceProvider( locator, type );
			} );
		}

		public void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}

		public void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}
	}
}