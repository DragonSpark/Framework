using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Application
{
    public class ApplicationConfigurationCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			AppDomain.CurrentDomain.With( x =>
			{
				var exceptionHandler = container.TryResolve<IExceptionHandler>();
				exceptionHandler.NotNull( y => x.UnhandledException += ( s, a ) => a.ExceptionObject.As<Exception>( z => y.Handle( z ) ) );
				x.DomainUnload += ( s, a ) => container.Resolve<IServiceLocator>().TryDispose();
			} );
		}
	}
}
