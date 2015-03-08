using System;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication
{
	public class DomainContextFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = container.TryResolve<AuthenticationService>().AsTo<FormsAuthentication, DomainContext>( x => x.DomainContext );
			return result;
		}
	}
}