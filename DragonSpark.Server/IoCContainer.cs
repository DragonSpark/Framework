using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace DragonSpark.Server
{
	public class IoCContainer : IDependencyResolver
	{
		readonly IServiceLocator locator;

		public IoCContainer(IServiceLocator locator)
		{
			this.locator = locator;
		}

		public object GetService(Type serviceType)
		{
			var result = locator.GetInstance(serviceType);
			return result;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			var result = locator.GetAllInstances(serviceType);
			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			disposing.IsTrue( locator.TryDispose );
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}
