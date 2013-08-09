using System;
using DragonSpark.IoC.Configuration;
using EventSourceProxy;
using Microsoft.Practices.Unity;

namespace DragonSpark.Logging.Configuration
{
	public class EventSourceProxyFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = EventSourceImplementer.GetEventSource( type );
			return result;
		}
	}
}