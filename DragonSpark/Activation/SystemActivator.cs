using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation
{
	class SystemActivator : IActivator
	{
		public static SystemActivator Instance
		{
			get { return InstanceField; }
		}	static readonly SystemActivator InstanceField = new SystemActivator();

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