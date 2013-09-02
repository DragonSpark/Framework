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
			var result = locator.GetInstance( serviceType );
			return result;
		}

		public override IEnumerable<object> GetServices( Type serviceType )
		{
			var result = locator.GetAllInstances( serviceType );
			return result;
		}

		public override void Register( Type serviceType, Func<object> activator )
		{
			var target = locator ?? ServiceLocator.Current;
			target.RegisterFactory( serviceType, activator );
		}

		public override void Register( Type serviceType, IEnumerable<Func<object>> activators )
		{
			Register( serviceType, () => activators.Select( x => x() ).ToArray() );
		}

		protected override void Dispose( bool disposing )
		{
			disposing.IsTrue( locator.TryDispose );
			base.Dispose( disposing );
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}
