using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Logging
{
	public class TraceListenerFactory : GenericItemFactoryBase
	{
		public string ListenerName { get; set; }

		protected override TItem Create<TItem>( IUnityContainer container, string buildName )
		{
			var result = container.Resolve<TraceListenerLocator>().Locate<TItem>( ListenerName ?? buildName );
			return result;
		}
	}
}