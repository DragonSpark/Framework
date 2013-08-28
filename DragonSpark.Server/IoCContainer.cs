using DragonSpark.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace DragonSpark.Server
{
	public class IoCContainer : DefaultDependencyResolver, IDependencyResolver
	{
		readonly IServiceLocator locator;

		public IoCContainer( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public override object GetService( Type serviceType )
		{
			var result = locator.GetInstance( serviceType ) ?? base.GetService( serviceType );
			return result;
		}

		public override IEnumerable<object> GetServices( Type serviceType )
		{
			var result = locator.GetAllInstances( serviceType ).Concat( base.GetServices( serviceType ) ?? Enumerable.Empty<object>() ).ToArray();
			return result;
		}

		protected override void Dispose( bool disposing )
		{
			disposing.IsTrue( locator.TryDispose );
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}
