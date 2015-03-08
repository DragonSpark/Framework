using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication.Configuration
{
	public class FaultBehavior : BehaviorExtensionElement, IEndpointBehavior
	{
		class SilverlightFaultMessageInspector : IDispatchMessageInspector
		{
			public void BeforeSendReply( ref Message reply, object correlationState )
			{
				if ( reply.IsFault )
				{
					// Here the response code is changed to 200.
					reply.Properties[ HttpResponseMessageProperty.Name ] = new HttpResponseMessageProperty { StatusCode = HttpStatusCode.OK };
				}
			}

			public object AfterReceiveRequest( ref Message request, IClientChannel channel, InstanceContext instanceContext )
			{
				return null;
			}
		}

		public void ApplyDispatchBehavior( ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher )
		{
			endpointDispatcher.DispatchRuntime.MessageInspectors.Add( new SilverlightFaultMessageInspector() );
		}

		public void AddBindingParameters( ServiceEndpoint endpoint, BindingParameterCollection bindingParameters )
		{}

		public void ApplyClientBehavior( ServiceEndpoint endpoint, ClientRuntime clientRuntime )
		{}

		public void Validate( ServiceEndpoint endpoint )
		{}

		public override Type BehaviorType
		{
			get { return typeof(FaultBehavior); }
		}

		protected override object CreateBehavior()
		{
			return new FaultBehavior();
		}
	}
}