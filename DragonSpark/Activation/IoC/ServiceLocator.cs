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
using System.Reflection;

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
			var basic = key == null && !serviceType.GetTypeInfo().IsInterface;
			if ( basic || Container.IsRegistered( serviceType, key ) )
			{
				return Container.TryResolve( serviceType, key );
			}

			Container.Logger().Warning( string.Format( Resources.ServiceLocator_NotRegistered, serviceType, key ?? Resources.Activator_None ) );
			return null;
		}

		[Reference]
		public IUnityContainer Container { get; }

		protected virtual void Dispose( bool disposing )
		{
			
		}
	}
}