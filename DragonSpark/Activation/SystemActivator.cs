using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.Utility;

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
			var result = info.IsValueType || ( !info.IsInterface && parameters.NotNull().Select( o => o.GetType() ).ToArray().Transform( types => info.DeclaredConstructors.Any( c => !c.IsStatic && TypeReflectionExtensions.ParametersMatch( c.GetParameters(), types ) )  ) );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).WithDefaults();
			return result;
		}
	}
}