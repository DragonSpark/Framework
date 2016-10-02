using DragonSpark.Application;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem.Generics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class TypeAdapter
	{
		readonly static Func<Type, bool> Specification = ApplicationTypeSpecification.Default.ToSpecificationDelegate();
		readonly static Func<Type, IEnumerable<Type>> Expand = ExpandInterfaces;
		readonly Func<Type, bool> isAssignableFrom;
		
		readonly Func<Type, ImmutableArray<MethodMapping>> methodMapper;
		readonly Func<Type, Type[]> getTypeArguments;
		public TypeAdapter( Type referenceType ) : this( referenceType, referenceType.GetTypeInfo() ) {}

		public TypeAdapter( TypeInfo info ) : this( info.AsType(), info ) {}

		public TypeAdapter( Type referenceType, TypeInfo info )
		{
			ReferenceType = referenceType;
			Info = info;
			methodMapper = new DecoratedSourceCache<Type, ImmutableArray<MethodMapping>>( new MethodMapper( this ).Get ).Get;
			GenericFactoryMethods = new GenericStaticMethodFactories( ReferenceType );
			GenericCommandMethods = new GenericStaticMethodCommands( ReferenceType );
			isAssignableFrom = new IsInstanceOfTypeOrDefinitionCache( this ).Get;
			getTypeArguments = new GetTypeArgumentsForCache( this ).Get;
		}

		public Type ReferenceType { get; }

		public TypeInfo Info { get; }

		public GenericStaticMethodFactories GenericFactoryMethods { get; }
		public GenericStaticMethodCommands GenericCommandMethods { get; }

		public Type[] WithNested() => Info.Append( Info.DeclaredNestedTypes ).AsTypes().Where( Specification ).ToArray();

		public ConstructorInfo FindConstructor( params Type[] parameterTypes ) => 
				InstanceConstructors.Default.Get( Info )
				.Introduce( parameterTypes, tuple => CompatibleArgumentsSpecification.Default.Get( tuple.Item1 ).IsSatisfiedBy( tuple.Item2 ) )
				.SingleOrDefault();

		public bool IsAssignableFrom( Type other ) => isAssignableFrom( other );
		bool IsAssignableFromBody( Type parameter ) => Info.IsGenericTypeDefinition && parameter.Adapt().IsGenericOf( ReferenceType ) || Info.IsAssignableFrom( parameter.GetTypeInfo() );
		class IsInstanceOfTypeOrDefinitionCache : ArgumentCache<Type, bool>
		{
			public IsInstanceOfTypeOrDefinitionCache( TypeAdapter owner ) : base( owner.IsAssignableFromBody ) {}
		}

		public bool IsInstanceOfType( object instance ) => IsAssignableFrom( instance.GetType() );
		
		public Assembly Assembly => Info.Assembly;

		public IEnumerable<Type> GetHierarchy( bool includeRoot = false )
		{
			yield return ReferenceType;
			var current = Info.BaseType;
			while ( current != null )
			{
				if ( current != typeof(object) || includeRoot )
				{
					yield return current;
				}
				current = current.GetTypeInfo().BaseType;
			}
		}

		[Freeze]
		public Type GetEnumerableType() => InnerType( GetHierarchy(), types => types.Only(), i => i.Adapt().IsGenericOf( typeof(IEnumerable<>) ) );

		[Freeze]
		public Type GetInnerType() => InnerType( GetHierarchy(), types => types.Only() );

		static Type InnerType( IEnumerable<Type> hierarchy, Func<Type[], Type> fromGenerics, Func<TypeInfo, bool> check = null )
		{
			foreach ( var type in hierarchy )
			{
				var info = type.GetTypeInfo();
				var result = info.IsGenericType && info.GenericTypeArguments.Any() && ( check?.Invoke( info ) ?? true ) ? fromGenerics( info.GenericTypeArguments ) :
					type.IsArray ? type.GetElementType() : null;
				if ( result != null )
				{
					return result;
				}
			}
			return null;
		}

		public Type[] GetTypeArgumentsFor( Type implementationType ) => getTypeArguments( implementationType );
		Type[] GetTypeArgumentsForBody( Type implementationType ) => GetImplementations( implementationType ).First().GenericTypeArguments;
		class GetTypeArgumentsForCache : ArgumentCache<Type, Type[]>
		{
			public GetTypeArgumentsForCache( TypeAdapter owner ) : base( owner.GetTypeArgumentsForBody ) {}
		}

		[Freeze]
		public Type[] GetImplementations( Type genericDefinition, bool includeInterfaces = true )
		{
			var result = ReferenceType.Append( includeInterfaces ? Expand( ReferenceType ) : Items<Type>.Default )
							 .Distinct()
							 .Introduce( genericDefinition, tuple =>
															{
																var first = tuple.Item1.GetTypeInfo();
																var second = tuple.Item2.GetTypeInfo();
																var match = first.IsGenericType && second.IsGenericType && tuple.Item1.GetGenericTypeDefinition() == tuple.Item2.GetGenericTypeDefinition();
																return match;
															} )
							 .Fixed();
			return result;
		}

		public ImmutableArray<MethodMapping> GetMappedMethods<T>() => GetMappedMethods( typeof(T) );
		public ImmutableArray<MethodMapping> GetMappedMethods( Type interfaceType ) => methodMapper( interfaceType );
		

		[Freeze]
		public bool IsGenericOf( Type genericDefinition ) => IsGenericOf( genericDefinition, true );

		[Freeze]
		public bool IsGenericOf( Type genericDefinition, bool includeInterfaces ) => GetImplementations( genericDefinition, includeInterfaces ).Any();

		[Freeze]
		public Type[] GetAllInterfaces() => Expand( ReferenceType ).ToArray();

		static IEnumerable<Type> ExpandInterfaces( Type target ) => target.Append( target.GetTypeInfo().ImplementedInterfaces.SelectMany( Expand ) ).Where( x => x.GetTypeInfo().IsInterface ).Distinct();

		[Freeze]
		public ImmutableArray<Type> GetEntireHierarchy() => Expand( ReferenceType ).Union( GetHierarchy() ).ToImmutableArray();
	}
}