using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication
{
	/// <summary>
	/// See http://msdn.microsoft.com/en-us/library/dd470096(VS.96).aspx
	/// </summary>
	public class WcfSilverlightFaultBehavior : IDispatchMessageInspector
	{
		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			if (reply.IsFault)
			{
				var property = new HttpResponseMessageProperty { StatusCode = System.Net.HttpStatusCode.OK };

				// Here the response code is changed to 200.
				reply.Properties[HttpResponseMessageProperty.Name] = property;
			}
		}

		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			// Do nothing to the incoming message.
			return null;
		}
	}
}
