using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.IoC;

namespace DragonSpark.Application.Communication
{
    public class DomainContextCookieInjection : BehaviorPolicyBase<DomainContext>
    {
        readonly SharedCookieHttpBehavior cookieHttpBehavior;

        public DomainContextCookieInjection( SharedCookieHttpBehavior cookieHttpBehavior )
        {
            this.cookieHttpBehavior = cookieHttpBehavior;
        }

        protected override void Apply( DomainContext target )
        {
            var channelFactoryProperty = target.DomainClient.GetType().GetProperty( "ChannelFactory" );
            if ( channelFactoryProperty == null )
            {
                throw new InvalidOperationException(
                    "There is no 'ChannelFactory' property on the DomainClient." );
            }
            var factory = (ChannelFactory)channelFactoryProperty.GetValue( target.DomainClient, null );
            ( (CustomBinding)factory.Endpoint.Binding ).Elements.Insert( 0, new HttpCookieContainerBindingElement() );
            factory.Endpoint.Behaviors.Add( cookieHttpBehavior );
        }
    }
}