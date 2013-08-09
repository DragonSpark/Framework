using System;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication.Entity
{
	public class DomainServiceFactoryConfiguration : IContainerConfigurationCommand, IDomainServiceFactory
	{
		IUnityContainer Container { get; set; }

		public void Configure( IUnityContainer container )
		{
			Container = container;
			DomainService.Factory = this;
		}

		public DomainService CreateDomainService( Type domainServiceType, DomainServiceContext context )
		{
			lock ( Container )
			{
				try
				{
					var result = Container.Resolve( domainServiceType ).To<DomainService>();
					result.Initialize( context );
					return result;
				}
				catch ( Exception error )
				{
					Exception rethrow;
					if ( ExceptionPolicy.HandleException( error, ExceptionShielding.DefaultExceptionPolicy, out rethrow ) )
					{
						throw rethrow;
					}
				}
				return null;
			}
		}

		public void ReleaseDomainService( DomainService domainService )
		{
			domainService.Dispose();
		}
	}
}