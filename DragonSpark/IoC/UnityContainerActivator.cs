using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	[Singleton( typeof(IActivator), Priority = Priority.Lowest )]
	public class UnityContainerActivator : IActivator
	{
		readonly IUnityContainer container;

		public UnityContainerActivator( IUnityContainer container )
		{
			this.container = container;
		}

		public TResult CreateInstance<TResult>( Type type, string name = null )
		{
			var result = container.Resolve( type, name ).To<TResult>();
			return result;
		}

		public TResult Create<TResult>( params object[] parameters )
		{
			var result = container.Create<TResult>( parameters );
			return result;
		}
	}
}