using DragonSpark.Activation;
using System;
using System.Linq;

namespace DragonSpark.Testing.TestObjects
{
	public class Activator : IActivator
	{
		public bool CanActivate( Type type, string name )
		{
			return true;
		}

		public object Activate( Type type, string name = null )
		{
			object result = type == typeof(Object) ? new Object { Name = name ?? "DefaultActivation" } : null;
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = type == typeof(Item) ? new Item { Parameters = parameters } : null;
			return result;
		}
	}
}