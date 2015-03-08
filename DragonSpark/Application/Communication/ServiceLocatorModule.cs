using System;
using System.Web;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication
{
    public class ServiceLocatorModule<TFactory> : IServiceLocatorModule where TFactory : Factory<IServiceLocator>
	{
		public void Init( HttpApplication context )
		{
			lock ( context.Application )
			{
				ServiceLocator = Activator.CreateInstance<TFactory>().Create();
			}
		}

		public void Dispose()
		{
			/*serviceLocator.TryDispose();
			serviceLocator = null;*/
		}

		public IServiceLocator ServiceLocator { get; private set; }	
	}
}