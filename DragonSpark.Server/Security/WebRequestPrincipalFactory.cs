using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Server.Security
{
	public class WebRequestPrincipalFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = ServerContext.Current.User;
			return result;
		}
	}
}