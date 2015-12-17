using DragonSpark.Extensions;
using System;
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
			var result = info.IsValueType || Coerce( type, parameters ) != null;
			return result;
		}

		static object[] Coerce( Type type, object[] parameters )
		{
			var candidates = new[] { parameters, parameters.NotNull() };
			var extended = type.Adapt();
			var result = candidates.Select( objects => objects.Fixed() ).FirstOrDefault( x => extended.FindConstructor( x ) != null );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var args = Coerce( type, parameters );
			var result = System.Activator.CreateInstance( type, args ).BuildUp();
			return result;
		}
	}
}