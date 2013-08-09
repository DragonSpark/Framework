using System;
using System.Web;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Web.Security
{
	public class WebRequestPrincipalFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = HttpContext.Current.User;
			return result;
		}
	}
}