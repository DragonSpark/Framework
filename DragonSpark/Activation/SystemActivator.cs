using DragonSpark.Extensions;
using Microsoft.Practices.Unity.Utility;
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
			var result = info.IsValueType || ( !info.IsInterface && parameters.NotNull().Select( o => o.GetType() ).ToArray().Transform( types => info.DeclaredConstructors.Any( c => !c.IsStatic && Match( c.GetParameters(), types ) ) ) );
			return result;
		}

		static bool Match(IReadOnlyCollection<ParameterInfo> parameters, IReadOnlyList<Type> closedConstructorParameterTypes)
        {
            var result = parameters.Count == closedConstructorParameterTypes.Count && !parameters.Where( ( t, i ) => !t.ParameterType.IsAssignableFrom( closedConstructorParameterTypes[i] ) ).Any();
			return result;
        }

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).WithDefaults();
			return result;
		}
	}
}