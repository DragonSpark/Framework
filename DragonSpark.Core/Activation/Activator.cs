using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation
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
			var result = (TResult)Create( typeof(TResult), parameters );
			return result;
		}

		public static object Create( Type type, params object[] parameters )
		{
			var result = ResolveActivator().InvokeGeneric( "Create", new [] { type }, parameters );
			return result;
		}
	} 
}