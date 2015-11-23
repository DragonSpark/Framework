using DragonSpark.Extensions;
using System;
using System.Collections;
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
			var result = info.IsValueType || new TypeSupport( info ).FindConstructor( parameters ) != null;
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = System.Activator.CreateInstance( type, parameters ).BuildUp();
			return result;
		}
	}

	public class TypeSupport
	{
		readonly Type type;
		readonly TypeInfo info;

		public TypeSupport( Type type ) : this( type.GetTypeInfo() )
		{
			this.type = type;
		}

		public TypeSupport( TypeInfo info )
		{
			this.info = info;
		}

		public object GetDefaultValue()
		{
			var result = info.IsValueType && Nullable.GetUnderlyingType( type ) == null ? System.Activator.CreateInstance( type ) : null;
			return result;
		}

		public ConstructorInfo FindConstructor( object[] parameters )
		{
			var types = parameters.Select( o => o.Transform( o1 => o1.GetType() ) ).ToArray();
			var result = info.DeclaredConstructors.SingleOrDefault( c => !c.IsStatic && Match( c.GetParameters(), types ) );
			return result;
		}

		static bool Match( IReadOnlyCollection<ParameterInfo> parameters, IReadOnlyList<Type> parameterTypes )
		{
			var result = parameters.Count == parameterTypes.Count && !parameters.Where( ( t, i ) => !parameterTypes[i].Transform( t.ParameterType.Extend().IsAssignableFrom, () => true ) ).Any();
			return result;
		}

		public bool CanLocate<T>()
		{
			var result = typeof(T).Extend().CanLocate( type );
			return result;
		}

		public bool CanLocate( Type instanceType )
		{
			var extend = instanceType.Extend();
			var result  = extend.CanLocate() && info.IsAssignableFrom( extend.info );
			return result;
		}

		public bool CanLocate()
		{
		    var result = info.IsInterface || !info.IsAbstract;
			return result;
		}

		public bool IsAssignableFrom( Type other )
		{
			var result = info.IsAssignableFrom( other.GetTypeInfo() );
			return result;
		}

		public bool IsSubclass( Type other )
		{
			var result = info.IsSubclassOf( other );
			return result;
		}

		public Assembly Assembly => info.Assembly;

		public IEnumerable<Type> GetHierarchy( bool includeRoot = true )
		{
			var result = new List<Type> { type };
			var current = info.BaseType;
			while ( current != null )
			{
				if ( current != typeof(object) || includeRoot )
				{
					result.Add( current );
				}
				current = current.GetTypeInfo().BaseType;
			}
			return result;
		}

		public Type GetEnumerableType()
		{
			var result = InnerType( type, typeof(IEnumerable).GetTypeInfo().IsAssignableFrom );
			return result;
		}

		public Type GetInnerType()
		{
			return InnerType( type );
		}

		static Type InnerType( Type target, Func<TypeInfo, bool> check = null )
		{
			var info = target.GetTypeInfo();
			var result = info.IsGenericType && info.GenericTypeArguments.Any() && check.Transform( func => func( info ), () => true ) ? info.GenericTypeArguments.FirstOrDefault() :
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public Type[] GetAllInterfaces()
		{
			var result = ExpandInterfaces( type ).ToArray();
			return result;
		}

		static IEnumerable<Type> ExpandInterfaces( Type target )
		{
			var ti = target.GetTypeInfo();
			var result = target.Append( ti.ImplementedInterfaces.SelectMany( ExpandInterfaces ) ).Where( x => x.GetTypeInfo().IsInterface ).Distinct();
			return result;
		}
	}
}