using DragonSpark.Activation;
using System;

namespace DragonSpark.Testing.TestObjects
{
	public class Activator : IActivator
	{
		public bool CanActivate( Type type, string name )
		{
			return true;
		}

		public TResult CreateInstance<TResult>( Type type, string name )
		{
			object item = type == typeof(Object) ? new Object { Name = name ?? "DefaultActivation" } : null;
			var result = item != null ? (TResult)item : default( TResult );
			return result;
		}

		public TResult Create<TResult>( params object[] parameters )
		{
			object item = typeof(TResult) == typeof(Item) ? new Item { Parameters = parameters } : null;
			var result = item != null ? (TResult)item : default( TResult );
			return result;
		}
	}
}