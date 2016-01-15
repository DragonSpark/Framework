using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Threading;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Activation
{
	[Synchronized]
	public class SystemActivator : IActivator
	{
		public static SystemActivator Instance { get; } = new SystemActivator();

		public bool CanActivate( Type type, string name ) => CanConstruct( type );

		public object Activate( Type type, string name = null ) => Construct( type );

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var info = type.GetTypeInfo();
			var result = info.IsValueType || Coerce( type, parameters ) != null;
			return result;
		}

		static object[] Coerce( Type type, object[] parameters )
		{
			var candidates = new[] { parameters, parameters.NotNull() };
			var adapter = type.Adapt();
			var result = candidates.Select( objects => objects.Fixed() ).FirstOrDefault( x => adapter.FindConstructor( x ) != null );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var args = Coerce( type, parameters ) ?? new object[0];

			var activator = type.Adapt().FindConstructor( args ).With( GetActivator );

			var result = activator( args );
			return result;
		}

		delegate object ObjectActivator( params object[] args );

		[Freeze]
		static ObjectActivator GetActivator( ConstructorInfo ctor )
		{
			var paramsInfo = ctor.GetParameters();
			var param = Expression.Parameter( typeof(object[]), "args" );

			var argsExp = new Expression[paramsInfo.Length];
			for ( var i = 0; i < paramsInfo.Length; i++ )
			{
				var paramAccessorExp = Expression.ArrayIndex( param, Expression.Constant( i ) );
				argsExp[i] = Expression.Convert( paramAccessorExp, paramsInfo[i].ParameterType );
			}

			var newExp = Expression.New( ctor, argsExp );

			var lambda = Expression.Lambda( typeof(ObjectActivator), newExp, param );

			//compile it
			var compiled = (ObjectActivator)lambda.Compile();
			return compiled;
		}
	}
}