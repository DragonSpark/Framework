using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class ServiceLocationBehavior : IServiceBehavior, IInstanceProvider
	{
		readonly Func<IServiceLocator> serviceLocator;

		public ServiceLocationBehavior( Func<IServiceLocator> serviceLocator )
		{
			this.serviceLocator = serviceLocator;
		}

		object IInstanceProvider.GetInstance( InstanceContext instanceContext, Message message )
		{
			var result = GetInstance( instanceContext );
			return result;
		}

		object GetInstance( InstanceContext instanceContext )
		{
			var type = instanceContext.Host.Transform( x => x.Description.ServiceType );
			var result = serviceLocator().GetInstance( type );
			return result;
		}

		object IInstanceProvider.GetInstance( InstanceContext instanceContext )
		{
			var result = GetInstance( instanceContext );
			return result;
		}

		void IInstanceProvider.ReleaseInstance( InstanceContext instanceContext, object instance )
		{
			instance.TryDispose();
		}

		void IServiceBehavior.Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}

		void IServiceBehavior.AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}

		void IServiceBehavior.ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			// serviceHostBase.Extensions.Add( new ServiceLocationExtension( serviceLocator ) );
			serviceDescription.Name.NotNull( name => serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany( x => x.Endpoints ).Apply( x =>
			{
			    x.DispatchRuntime.InstanceProvider = this;
			} ) );
		}
	}
}