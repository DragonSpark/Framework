using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace DragonSpark.Application.Communication.Channels
{
    public partial class ChannelFactory<TSync> where TSync : class
    {
        /// <summary>
        /// Gets the credentials used to initialise the produced channel
        /// </summary>
        public ClientCredentials Credentials
        {
            get { return Factory.Credentials; }
        }

        /// <summary>
        /// generated an async channel for the specified sync type
        /// </summary>        
        /// <returns>the channel created by the factory</returns>
        /// <remarks>
        /// for the ServiceReferences.ClientConfig file: the async type will be generated in a .Async sub namespace of the TSync type
        /// if TSync is for example in MyApplication.MyNamespace.TSync
        /// the async version will be created with the name MyApplication.MyNamespace.Async.TSync
        /// use this async fullyqualified name in the ServiceReferences.ClientConfig for the contract attribute
        /// </remarks>
        public Channel<TSync> CreateChannel()
        {
            return CreateAsyncChannel<TSync>( Factory );
        }

        /// <summary>
        /// Creates a channel that is used to send messages to a service at a specific endpoint address.
        /// </summary>
        /// <param name="address">the address</param>
        /// <returns>the channel created by the factory</returns>
        /// <remarks>
        /// for the ServiceReferences.ClientConfig file: the async type will be generated in a .Async sub namespace of the TSync type
        /// if TSync is for example in MyApplication.MyNamespace.TSync
        /// the async version will be created with the name MyApplication.MyNamespace.Async.TSync
        /// use this async fullyqualified name in the ServiceReferences.ClientConfig for the contract attribute
        /// </remarks>
        public Channel<TSync> CreateChannel( EndpointAddress address )
        {
            return CreateAsyncChannel<TSync>( Factory, address );
        }

        /// <summary>
        /// Creates a channel that is used to send messages to a service at a specific endpoint address through a specified transport address.
        /// </summary>
        /// <param name="address">The EndpointAddress that provides the location of the service.</param>
        /// <param name="via">The Uri that contains the transport address to which the channel sends messages.</param>
        /// <returns>the channel created by the factory</returns>
        /// <remarks>
        /// for the ServiceReferences.ClientConfig file: the async type will be generated in a .Async sub namespace of the TSync type
        /// if TSync is for example in MyApplication.MyNamespace.TSync
        /// the async version will be created with the name MyApplication.MyNamespace.Async.TSync
        /// use this async fullyqualified name in the ServiceReferences.ClientConfig for the contract attribute
        /// </remarks>
        public Channel<TSync> CreateChannel( EndpointAddress address, Uri via )
        {
            return CreateAsyncChannel<TSync>( Factory, address, via );
        }

        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <typeparam name="T">the synchronous interface type of the service</typeparam>        
        /// <param name="channelFactory">the channel factory instace for the channel creation</param>
        /// <param name="arguments">arguments to pass to the CreateChannel method of the channel factory</param>
        /// <returns>an instance of the  asyncronous channel</returns>
        static Channel<T> CreateAsyncChannel<T>( ChannelFactory channelFactory, params object[] arguments )
        {
            var channel = CreateAsynchronousChannel( channelFactory, arguments );
            return new Channel<T>( channel );
        }

        public IChannel CreateAsynchronousChannel( params object[] arguments )
        {
            var result = CreateAsynchronousChannel( Factory, arguments );
            return result;
        }

        public static IChannel CreateAsynchronousChannel( ChannelFactory channelFactory, params object[] arguments )
        {
            var factoryType = channelFactory.GetType();
            var createChannelMethod = factoryType.GetMethod( "CreateChannel", arguments.Select( x => x.GetType() ).ToArray() );
            var channel = (IChannel)createChannelMethod.Invoke( channelFactory, arguments );
            return channel;
        }
    }
}
