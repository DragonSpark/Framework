using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace DragonSpark.Application.Communication.Channels
{
    public class DuplexChannelFactory<T> : ChannelFactoryBase<T> where T : class
    {
        readonly InstanceContext callbackInstance;

        public DuplexChannelFactory( InstanceContext callbackInstance, string endpointConfigurationName = "*" ) : base( callbackInstance, endpointConfigurationName )
        {
            this.callbackInstance = callbackInstance;
        }

        protected override IChannel CreateChannelInstance()
        {
            return CreateChannel( Factory, callbackInstance );
        }

        protected override Type FactoryType
        {
            get { return typeof(System.ServiceModel.DuplexChannelFactory<>); }
        }
    }

    public class ChannelFactory<T> : ChannelFactoryBase<T> where T : class
    {
        public ChannelFactory( string endpointConfigurationName = "*" ) : base( endpointConfigurationName )
        {}

        protected override Type FactoryType
        {
            get { return typeof(System.ServiceModel.ChannelFactory<>); }
        }
    }

    public abstract class ChannelFactoryBase<T> : ChannelFactory where T : class
    {
        readonly ChannelFactory factory;

        protected ChannelFactoryBase( params object[] arguments )
        {
            factory = CreateInnerFactory( arguments );
        }

        public T CreateChannel()
        {
            var channel = CreateChannelInstance();
            var proxyTypeFor = TypeGenerator.Instance.GenerateProxyTypeFor<T>();
            var result = (T)Activator.CreateInstance( proxyTypeFor, channel );
            return result;
        }

        protected virtual IChannel CreateChannelInstance()
        {
            return CreateChannel( Factory );
        }

        protected override IChannelFactory CreateFactory()
        {
            return factory;
        }

        protected override ServiceEndpoint CreateDescription()
        {
            return factory.Endpoint;
        }

        protected ChannelFactory Factory
        {
            get { return factory; }
        }

        protected static IChannel CreateChannel( ChannelFactory channelFactory, params object[] arguments )
        {
            var factoryType = channelFactory.GetType();
            var createChannelMethod = factoryType.GetMethod( "CreateChannel", arguments.Select( x => x.GetType() ).ToArray() );
            var channel = (IChannel)createChannelMethod.Invoke( channelFactory, arguments );
            return channel;
        }

        ChannelFactory CreateInnerFactory(params object[] arguments)
        {
            var asyncType = TypeGenerator.Instance.GenerateAsyncInterfaceFor<T>();
            var factoryType = DetermineFactoryType( asyncType );
            var channelFactory = (ChannelFactory)Activator.CreateInstance( factoryType, arguments );
            return channelFactory;
        }

        Type DetermineFactoryType(Type asyncType)
        {
            var factoryType = FactoryType.MakeGenericType( new[] { asyncType } );
            return factoryType;
        }

        protected abstract Type FactoryType { get; }
    }
}