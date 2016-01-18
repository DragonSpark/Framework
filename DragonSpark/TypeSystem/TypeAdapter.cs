using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class TypeAdapter
	{
		readonly System.Type type;
		readonly TypeInfo info;

		public TypeAdapter( [Required]System.Type type ) : this( type.GetTypeInfo() )
		{
			this.type = type;
		}

		public TypeAdapter( [Required]TypeInfo info )
		{
			this.info = info;
		}

		public object GetDefaultValue() => info.IsValueType && Nullable.GetUnderlyingType( type ) == null ? Activator.CreateInstance( type ) : null;

		public ConstructorInfo FindConstructor( params object[] parameters ) => FindConstructor( parameters.Select( o => o?.GetType() ).ToArray() );

		public ConstructorInfo FindConstructor( params System.Type[] parameterTypes ) => info.DeclaredConstructors.SingleOrDefault( c => c.IsPublic && !c.IsStatic && Match( c.GetParameters(), parameterTypes ) );

		static bool Match( IReadOnlyCollection<ParameterInfo> parameters, IReadOnlyCollection<System.Type> provided )
		{
			var result = 
				provided.Count >= parameters.Count( info => !info.IsOptional ) && 
				provided.Count <= parameters.Count && 
				parameters.Select( ( t, i ) => provided.ElementAtOrDefault( i ).With( t.ParameterType.Adapt().IsAssignableFrom, () => i < provided.Count || t.IsOptional ) ).All( b => b );
			return result;
		}

		public object Qualify( object instance ) => instance.With( o => info.IsAssignableFrom( o.GetType().GetTypeInfo() ) ? o : GetCaster( o.GetType() ).With( caster => caster.Invoke( null, new[] { o } ) ) );

		public bool IsAssignableFrom( TypeInfo other ) => IsAssignableFrom( other.AsType() );

		public bool IsAssignableFrom( System.Type other ) => info.IsAssignableFrom( other.GetTypeInfo() ) || GetCaster( other ) != null;

		public bool IsInstanceOfType( object context ) => context.With( o => IsAssignableFrom( o.GetType() ) );

		MethodInfo GetCaster( System.Type other ) => info.DeclaredMethods.SingleOrDefault( method => method.Name == "op_Implicit" && method.GetParameters().First().ParameterType.GetTypeInfo().IsAssignableFrom( other.GetTypeInfo() ) );

		public Assembly Assembly => info.Assembly;

		public System.Type[] GetHierarchy( bool includeRoot = true )
		{
			var result = new List<System.Type> { type };
			var current = info.BaseType;
			while ( current != null )
			{
				if ( current != typeof(object) || includeRoot )
				{
					result.Add( current );
				}
				current = current.GetTypeInfo().BaseType;
			}
			return result.ToArray();
		}

		public System.Type GetEnumerableType() => InnerType( type, types => types.FirstOrDefault(), typeof( IEnumerable ).GetTypeInfo().IsAssignableFrom );

		// public Type GetResultType() => type.Append( ExpandInterfaces( type ) ).FirstWhere( t => InnerType( t, types => types.LastOrDefault() ) );

		public System.Type GetInnerType() => InnerType( type, types => types.Only() );

		static System.Type InnerType( System.Type target, Func<System.Type[], System.Type> fromGenerics, Func<TypeInfo, bool> check = null )
		{
			var info = target.GetTypeInfo();
			var result = info.IsGenericType && info.GenericTypeArguments.Any() && check.With( func => func( info ), () => true ) ? fromGenerics( info.GenericTypeArguments ) :
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public bool IsGenericOf<T>() => IsGenericOf( typeof( T ).GetGenericTypeDefinition() );

		[Freeze]
		public bool IsGenericOf( System.Type genericDefinition ) =>
			type.Append( ExpandInterfaces( type ) ).AsTypeInfos().Any( typeInfo => typeInfo.IsGenericType && genericDefinition.GetTypeInfo().IsGenericType && typeInfo.GetGenericTypeDefinition() == genericDefinition.GetGenericTypeDefinition() );

		public System.Type[] GetAllInterfaces() => ExpandInterfaces( type ).ToArray();

		static IEnumerable<System.Type> ExpandInterfaces( System.Type target ) => target.Append( target.GetTypeInfo().ImplementedInterfaces.SelectMany( ExpandInterfaces ) ).Where( x => x.GetTypeInfo().IsInterface ).Distinct();

		public System.Type[] GetEntireHierarchy() => ExpandInterfaces( type ).Union( GetHierarchy( false ) ).Distinct().ToArray();
	}
}