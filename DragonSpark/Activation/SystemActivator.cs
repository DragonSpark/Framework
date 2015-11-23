using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			var result = info.IsValueType || ( !info.IsInterface && parameters.Select( o => o.Transform( o1 => o1.GetType() ) ).ToArray().Transform( types => info.DeclaredConstructors.Any( c => !c.IsStatic && Match( c.GetParameters(), types ) ) ) );
			return result;
		}

		static bool Match( IReadOnlyCollection<ParameterInfo> parameters, IReadOnlyList<Type> parameterTypes )
		{
			var result = parameters.Count == parameterTypes.Count && !parameters.Where( ( t, i ) => !parameterTypes[i].Transform( t.ParameterType.IsAssignableFrom, () => true ) ).Any();
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).WithDefaults();
			return result;
		}
	}
}