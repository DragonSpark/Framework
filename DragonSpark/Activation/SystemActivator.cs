using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation
{
	public class SystemActivator : IActivator
	{
		public static SystemActivator Instance { get; } = new SystemActivator();

		public bool CanActivate( Type type, string name )
		{
			var result = type.CanActivate();
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = System.Activator.CreateInstance( type ).WithDefaults();
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).WithDefaults();
			return result;
		}
	}
}