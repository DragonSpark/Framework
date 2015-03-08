using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class FaultMessageInspector : IDispatchMessageInspector
	{
		object IDispatchMessageInspector.AfterReceiveRequest( ref Message request, IClientChannel channel,
		                                                      InstanceContext instanceContext )
		{
			// Do nothing to the incoming message
			return null;
		}

		void IDispatchMessageInspector.BeforeSendReply( ref Message reply, object correlationState )
		{
			if ( reply.IsFault )
			{
				var property = new HttpResponseMessageProperty { StatusCode = HttpStatusCode.OK };
				reply.Properties[ HttpResponseMessageProperty.Name ] = property;
			}
		}
	}
}