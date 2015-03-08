using System;

namespace DragonSpark.Runtime
{
	public static class Activator
	{
		public static TResult CreateInstance<TResult>( Type type )
		{
			var result = CreateNamedInstance<TResult>( type, null );
			return result;
		}

		public static TResult CreateNamedInstance<TResult>( Type type, string name )
		{
			var result = ResolveActivator().CreateInstance<TResult>( type, name );
			return result;
		}

		static IActivator ResolveActivator()
		{
			var result = ServiceLocation.Locate<IActivator>() ?? SystemActivator.Instance;
			return result;
		}

		public static TResult Create<TResult>( params object[] parameters )
		{
			var result = ResolveActivator().Create<TResult>( parameters );
			return result;
		}
	} 
}