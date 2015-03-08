using System.ServiceModel;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication.Configuration
{
	public class ServiceFactory : GenericItemFactoryBase
	{
		protected override TItem Create<TItem>( IUnityContainer container, string buildName )
		{
			var channelFactory = new ChannelFactory<TItem>( buildName ?? "*" );
			var result = channelFactory.CreateChannel();
			result.As<IClientChannel>( x =>
			{
				container.RegisterDisposable( x );
				x.Closed += ( sender, args ) => channelFactory.TryDispose();
			} );
			return result;
		}
	}
}