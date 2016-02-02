using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Activation.IoC
{
	[Disposable( ThrowObjectDisposedException = true )]
	public class ServiceLocator : ServiceLocatorImplBase
	{
		public ServiceLocator() : this( new UnityContainer() ) {}

		public ServiceLocator( [Required]IUnityContainer container )
		{
			Container = container;
		}

		public override IEnumerable<TService> GetAllInstances<TService>()
		{
			var enumerable = Container.IsRegistered<IEnumerable<TService>>() ? Container.Resolve<IEnumerable<TService>>() : Enumerable.Empty<TService>();
			var result = base.GetAllInstances<TService>().Union( enumerable ).ToArray();
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType ) => Container.ResolveAll( serviceType ).ToArray();

		protected override object DoGetInstance( Type serviceType, string key )
		{
			var result = Container.TryResolve( serviceType, key );
			if ( result == null && !Container.IsRegistered( serviceType, key ) )
			{
				Container.Logger().Warning( string.Format( Resources.ServiceLocator_NotRegistered, serviceType, key ?? Resources.Activator_None ) );
			}
			return result;
		}

		[Child]
		public IUnityContainer Container { get; }
	}
}