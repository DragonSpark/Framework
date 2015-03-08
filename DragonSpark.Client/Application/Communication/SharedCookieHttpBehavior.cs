using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication
{
    public class SharedCookieHttpBehavior : WebHttpBehavior, IClientMessageInspector
    {
        public override void ApplyClientBehavior( ServiceEndpoint endpoint, ClientRuntime clientRuntime )
        {
            clientRuntime.MessageInspectors.Add( this );
        }

        void IClientMessageInspector.AfterReceiveReply( ref Message reply, object correlationState )
        {}

        object IClientMessageInspector.BeforeSendRequest( ref Message request, IClientChannel channel )
        {
            var application = ServiceLocator.Current.GetInstance<IApplicationContext>();
            var cookieContainer = CookieContainerFactory.Instance.Create( application.Location );
            channel.GetProperty<IHttpCookieContainerManager>().CookieContainer = cookieContainer;
            return null;
        }
    }
}