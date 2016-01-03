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

		/*public PropertyInfo GetProperty( string name )
		{
			var result = type.GetPropertiesHierarchical().FirstOrDefault( propertyInfo => propertyInfo.Name == name );
			return result;
		}

		public FieldInfo GetField( string name )
		{
			var candidates = GetHierarchy( false ).SelectMany( t => new Func<string, FieldInfo>[] { t.GetTypeInfo().GetDeclaredField, t.GetRuntimeField } );
			var result = candidates.Select( func => func( name ) ).NotNull().FirstOrDefault();
			return result;
		}*/

		/*public ConstructorInfo[] GetConstructors()
		{
			var result = GetHierarchy().SelectMany( t => t.GetTypeInfo().DeclaredConstructors ).ToArray();
			return result;
		}*/

		public ConstructorInfo FindConstructor( params object[] parameters )
		{
			var types = parameters.Select( o => o?.GetType() ).ToArray();
			var result = FindConstructor( types );
			return result;
		}

		public ConstructorInfo FindConstructor( params Type[] parameterTypes )
		{
			var result = info.DeclaredConstructors.SingleOrDefault( c => c.IsPublic && !c.IsStatic && Match( c.GetParameters(), parameterTypes ) );
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

		/*public bool CanInstantiate( Type instanceType )
		{
			var result  = info.IsAssignableFrom( instanceType.GetTypeInfo() );
			return result;
		}*/

		public object Qualify( object instance )
		{
			var result = instance.With( o => info.IsAssignableFrom( o.GetType().GetTypeInfo() ) ? o : GetCaster( o.GetType() ).With( caster => caster.Invoke( null, new [] { o } ) ) );
			return result;
		}

		public bool IsAssignableFrom( TypeInfo other )
		{
			var result = IsAssignableFrom( other.AsType() );
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

		public Type[] GetHierarchy( bool includeRoot = true )
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
			return result.ToArray();
		}

		public Type GetEnumerableType()
		{
			var result = InnerType( type, types => types.FirstOrDefault(), typeof(IEnumerable).GetTypeInfo().IsAssignableFrom );
			return result;
		}

		public Type GetResultType()
		{
			return InnerType( type, types => types.LastOrDefault() );
		}

		public Type GetInnerType()
		{
			return InnerType( type, types => types.Only() );
		}

		static Type InnerType( Type target, Func<Type[], Type> fromGenerics, Func<TypeInfo, bool> check = null )
		{
			var info = target.GetTypeInfo();
			var result = info.IsGenericType && info.GenericTypeArguments.Any() && check.With( func => func( info ), () => true ) ? fromGenerics( info.GenericTypeArguments ) :
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public bool IsGenericOf<T>()
		{
			return IsGenericOf( typeof(T).GetGenericTypeDefinition() );
		}

		public bool IsGenericOf( Type genericDefinition )
		{
			var result = info.IsGenericType && genericDefinition.GetTypeInfo().IsGenericType && info.GetGenericTypeDefinition() == genericDefinition.GetGenericTypeDefinition();
			return result;
		}

		public Type GetConventionCandidate( Assembly[] applicationAssemblies = null )  // TODO: Replace with Assemblies.Current.
		{
			var result = 
				DetermineImplementor().With( typeInfo => typeInfo.AsType() )
				??
				type.GetTypeInfo().ImplementedInterfaces.ToArray().With( interfaces => interfaces.FirstOrDefault( i => type.Name.Contains( i.Name.TrimStart( 'I' ) ) ) 
					??
					type.Assembly().Append( applicationAssemblies.Fixed() ).Distinct().With( assemblies => interfaces.FirstOrDefault( t => assemblies.Contains( t.Assembly() ) ) )
				) ?? type;
			return result;
		}
		
		public TypeInfo DetermineImplementor()
		{
			var result = info.IsInterface ? type.Assembly().DefinedTypes.Where( IsAssignableFrom ).FirstOrDefault( i => !i.IsAbstract && i.Name.StartsWith( type.Name.TrimStart( 'I' ) ) ) : null;
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

		public Type[] GetEntireHierarchy()
		{
			var result = ExpandInterfaces( type ).Union( GetHierarchy( false ) ).Distinct().ToArray();
			return result;
		}
	}
}