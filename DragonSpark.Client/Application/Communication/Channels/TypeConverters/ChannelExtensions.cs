using System.ServiceModel;

namespace DragonSpark.Application.Communication.Channels.TypeConverters
{
    /// <summary>
    /// Helper extensions for the Channel
    /// </summary>
    public static class ChannelExtensions
    {
        /// <summary>
        /// provides a conversion of the Channel in a ContextChannel, useful to work with the OperationCOntext for example
        /// </summary>
        /// <typeparam name="TSync">the type of the async channel communication interface</typeparam>
        /// <param name="channel">the async channel to be converted</param>
        /// <returns>a IContextChannel version of the async comunication channel</returns>
        //HINT: probably it is better to duplicate the entire class hierarchy to enable the use with the 
        //OperationContextScope object so to have for example an AsyncOperationContextScope that natively operates with Channel instead of return on the WCF types
        //so to avoid misure of the internal channel object
        public static IContextChannel ToContextChannel<TSync>(this Channel<TSync> channel)
        {
            return channel.channel as IContextChannel;
        }
    }    
}
