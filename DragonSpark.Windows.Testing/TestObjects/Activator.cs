using System;
using DragonSpark.Activation;
using DragonSpark.Testing.Objects;

namespace DragonSpark.Windows.Testing.TestObjects
{
	public class Activator : IActivator
	{
		public bool CanActivate( Type type, string name )
		{
			return true;
		}

		public object Activate( Type type, string name = null )
		{
			object result = type == typeof(DragonSpark.Testing.Objects.Object) ? new DragonSpark.Testing.Objects.Object { Name = name ?? "DefaultActivation" } : null;
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			return true;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = type == typeof(Item) ? new Item { Parameters = parameters } : null;
			return result;
		}
	}
}