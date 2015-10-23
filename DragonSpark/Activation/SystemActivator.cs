using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation
{
	class SystemActivator : IActivator
	{
		public static SystemActivator Instance { get; } = new SystemActivator();

		public bool CanActivate( Type type, string name )
		{
			var result = type.CanActivate();
			return result;
		}

		public TResult CreateInstance<TResult>( Type type, string name )
		{
			var result = System.Activator.CreateInstance( type ).WithDefaults().To<TResult>();
			return result;
		}

		public TResult Create<TResult>( params object[] parameters )
		{
			var result = System.Activator.CreateInstance( typeof(TResult), parameters ).WithDefaults().To<TResult>();
			return result;
		}
	}
}