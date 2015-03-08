using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class EnsureWorkflowExtensionsBehavior : IServiceBehavior
	{
		void IServiceBehavior.Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}

		void IServiceBehavior.AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}

		void IServiceBehavior.ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			ApplyBehavior( serviceHostBase );
		}

		static void ApplyBehavior( ServiceHostBase serviceHostBase )
		{
			serviceHostBase.As<WorkflowServiceHost>( x => x.Extensions.Apply( x.WorkflowExtensions.Add ) );
		}
	}
}