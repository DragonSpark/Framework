using DragonSpark.Extensions;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class TypeAdapter
	{
		readonly Type type;
		readonly TypeInfo info;

		public TypeAdapter( Type type ) : this( type.GetTypeInfo() )
		{
			this.type = type;
		}

		public TypeAdapter( TypeInfo info )
		{
			this.info = info;
		}

		public object GetDefaultValue()
		{
			var result = info.IsValueType && Nullable.GetUnderlyingType( type ) == null ? Activator.CreateInstance( type ) : null;
			return result;
		}

		public ConstructorInfo FindConstructor( params object[] parameters )
		{
			var types = parameters.Select( o => o?.GetType() ).ToArray();
			var result = FindConstructor( types );
			return result;
		}

		public ConstructorInfo FindConstructor( params Type[] parameterTypes )
		{
			var result = info.DeclaredConstructors.SingleOrDefault( c => !c.IsStatic && Match( c.GetParameters(), parameterTypes ) );
			return result;
		}

		static bool Match( IReadOnlyCollection<ParameterInfo> parameters, IReadOnlyCollection<Type> provided )
		{
			var result = 
				provided.Count >= parameters.Count( info => !info.IsOptional ) && 
				provided.Count <= parameters.Count && 
				parameters.Select( ( t, i ) => provided.ElementAtOrDefault( i ).With( t.ParameterType.Adapt().IsAssignableFrom, () => i < provided.Count || t.IsOptional ) ).All( b => b );
			return result;
		}

		public Type GuardAsAssignable<T>( string name )
		{
			Guard.TypeIsAssignable( typeof(T), type, name );
			return type;
		}

		/*public bool CanLocate<T>()
		{
			var result = typeof(T).Adapt().CanLocate( type );
			return result;
		}*/

		public bool CanLocate( Type instanceType )
		{
			var extend = instanceType.Adapt();
			var result  = extend.CanLocate() && info.IsAssignableFrom( extend.info );
			return result;
		}

		bool CanLocate()
		{
			var result = info.IsInterface || !info.IsAbstract;
			return result;
		}

		public object Qualify( object instance )
		{
			var result = instance.With( o => info.IsAssignableFrom( o.GetType().GetTypeInfo() ) ? o : GetCaster( o.GetType() ).With( caster => caster.Invoke( null, new [] { o } ) ) );
			return result;
		}

		public bool IsAssignableFrom( Type other )
		{
			var result = info.IsAssignableFrom( other.GetTypeInfo() ) || GetCaster( other ) != null;
			return result;
		}

		public bool IsInstanceOfType( object context )
		{
			var result = context.With( o => IsAssignableFrom( o.GetType() ) );
			return result;
		}

		MethodInfo GetCaster( Type other )
		{
			return info.DeclaredMethods.SingleOrDefault( method => method.Name == "op_Implicit" && method.GetParameters().First().ParameterType.GetTypeInfo().IsAssignableFrom( other.GetTypeInfo() ) );
		}

		/*public bool IsSubclass( Type other )
		{
			var result = info.IsSubclassOf( other );
			return result;
		}*/

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
			var result = info.IsGenericType && info.GenericTypeArguments.Any() && check.With( func => func( info ), () => true ) ? info.GenericTypeArguments.FirstOrDefault() :
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public Type GetConventionCandidate()
		{
			var result = type.GetTypeInfo().ImplementedInterfaces.ToArray().With( interfaces => interfaces.FirstOrDefault( t => type.Name.Contains( t.Name.Substring( 1 ) ) ) ?? interfaces.Only() ) ?? type;
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

		public Type[] GetAllHierarchy()
		{
			var result = ExpandInterfaces( type ).Union( GetHierarchy( false ) ).Distinct().ToArray();
			return result;
		}
	}
}