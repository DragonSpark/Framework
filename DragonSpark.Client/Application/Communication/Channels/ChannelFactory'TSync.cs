using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DragonSpark.Application.Communication.Channels
{
    /// <summary>
    /// Creates aynchronous channel
    /// </summary>
    /// <typeparam name="TSync">type of the synchronous service interface</typeparam>
    public partial class ChannelFactory<TSync> where TSync : class
    {            
        /// <summary>
        /// Builds an async channel factory
        /// </summary>
        /// <param name="endpointConfigurationName">the configuration name (if not specified uses the default one)</param>
        public ChannelFactory(string endpointConfigurationName = "*")
        {
            Factory = MakeChannelFactoryInstance(endpointConfigurationName);
        }

        /// <summary>
        /// Builds an async channel factory
        /// </summary>
        /// <param name="binding">the binding to use</param>
        /// <param name="remoteAddress">the remote addess</param>
        public ChannelFactory(Binding binding, EndpointAddress remoteAddress)
        {
            Factory = MakeChannelFactoryInstance(binding, remoteAddress);
        }
        
        /// <summary>
        /// Builds an async channel factory
        /// </summary>
        /// <param name="endpointConfigurationName">the endpoint configuration name</param>
        /// <param name="remoteAddress">the remote adress</param>
        public ChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress)
        {
            Factory = MakeChannelFactoryInstance(endpointConfigurationName, remoteAddress);
        }

        /// <summary>
        /// Gets or sets the internal wcf channel factory
        /// </summary>
        public ChannelFactory Factory { get; private set; }       

        /// <summary>
        /// Makes an instance of the required channel factory type
        /// </summary>
        /// <param name="arguments">argument to pass to the internal channel factory constructor</param>
        /// <returns>an instance of the channel factory of for the required generated asynchronous type</returns>
        private static ChannelFactory MakeChannelFactoryInstance(params object[] arguments)
        {
            Type asyncType = GenerateAsyncType<TSync>();
            Type factoryType = GenerateFactoryType(asyncType);

            ChannelFactory channelFactory;

            channelFactory = (ChannelFactory)Activator.CreateInstance(factoryType, arguments);

            return channelFactory;
        }

        /// <summary>
        /// generates a channel factory for the asyncType
        /// </summary>
        /// <param name="asyncType">the type that represent the asyncrhounos version of the interface of the service</param>
        /// <returns>a type for the factory for the asyncType</returns>
        private static Type GenerateFactoryType(Type asyncType)
        {
            Type genericFactoryType = typeof(System.ServiceModel.ChannelFactory<>);
            Type factoryType = genericFactoryType.MakeGenericType(new[] { asyncType });
            return factoryType;
        }

        /// <summary>
        /// transforms a synchronous type in its asynchronous version
        /// </summary>
        /// <typeparam name="T">the synchronous version of the service interfce</typeparam>
        /// <returns>a type that represents the asynchronous version of T</returns>
        private static Type GenerateAsyncType<T>() where T : class
        {
            Type asynchType = TypeGenerator.Instance.GenerateAsyncInterfaceFor<T>();
            return asynchType;
        }
    }
}
