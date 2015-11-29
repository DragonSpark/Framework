using DragonSpark.Extensions;
using System;
using System.Reflection;
using DragonSpark.Runtime;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation
{
	public class SystemActivator : IActivator
	{
		public static SystemActivator Instance { get; } = new SystemActivator();

		public bool CanActivate( Type type, string name )
		{
			var result = CanConstruct( type );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = Construct( type );
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var info = type.GetTypeInfo();
			var result = info.IsValueType || new TypeExtension( info ).FindConstructor( parameters ) != null;
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).BuildUp();
			return result;
		}
	}
}