﻿using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.IoC
{
	public static class UnityContainerExtensions
	{
		public static TResult TryResolve<TResult>( this IUnityContainer target, string name = null )
		{
			try
			{
				var result = name.Transform( x => target.Resolve<TResult>( x ), () => target.Resolve<TResult>() );
				return result;
			}
			catch ( ResolutionFailedException )
			{
				return default( TResult );
			}
		}

		/*public static IEnumerable<TItem> ResolveAllRegistered<TItem>( this IUnityContainer target )
		{
			var result = target.ResolveAllRegistered( typeof(TItem) ).Cast<TItem>();
			return result;
		}

		public static IEnumerable<object> ResolveAllRegistered( this IUnityContainer target, Type type )
		{
			var result = target.Registrations.Where( x => type.IsAssignableFrom( x.RegisteredType ) ).Select( x => target.Resolve( x.RegisteredType, x.Name ) ).NotNull().Distinct().ToArray();
			return result;
		}*/

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used for convenience.")]
		public static Type ResolveType<TObject>( this IUnityContainer container, string name = null )
		{
			return ResolveType( container, typeof(TObject), name );
		}

		public static Type ResolveType( this IUnityContainer container, Type type, string name = null )
		{
			var extension = container.Configure<DragonSparkExtension>();
			if ( extension != null )
			{
				Type result;
				if ( extension.MappedTypes.TryGetValue( new NamedTypeBuildKey( type, name ), out result ) )
				{
					return result;
				}
				var registered = container.IsRegisteredOrMapped( type, name );
				return registered ? type : null;
			}
			return null;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used for convenience.")]
		public static IEnumerable<Type> ResolveMappings<TObject>( this IUnityContainer container )
		{
			return ResolveMappings( container, typeof(TObject), null );
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used for convenience.")]
		public static IEnumerable<Type> ResolveMappings<TObject>( this IUnityContainer container, string name )
		{
			return ResolveMappings( container, typeof(TObject), name );
		}

		public static IEnumerable<Type> ResolveMappings( this IUnityContainer container, Type type )
		{
			return ResolveMappings( container, type, null );
		}

		public static IEnumerable<Type> ResolveMappings( this IUnityContainer container, Type type, string name )
		{
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull( type, "type" );
			var extension = container.Configure<DragonSparkExtension>();
			if ( extension != null )
			{
				List<Type> result;
				if ( extension.Mappings.TryGetValue( new NamedTypeBuildKey( type, name ), out result ) )
				{
					return result;
				}
			}
			return new Type[0];
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used for convenience.")]
		public static bool IsRegisteredOrMapped<TObjectType>( this IUnityContainer container, string name = null )
		{
			var result = container.IsRegisteredOrMapped( typeof(TObjectType), name );
			return result;
		}

		public static bool IsRegisteredOrMapped( this IUnityContainer container, Type type, string name = null )
		{
			var result = container.IsRegistered( type, name ) || IsMapped( container, type, name );
			return result;
		}

		public static bool IsResolvable( this IUnityContainer container, Type type, string name = null )
		{
			var result = ( !type.IsInterface && !type.IsAbstract ) || container.IsRegisteredOrMapped( type, name );
			return result;
		}


		static bool IsMapped( IUnityContainer container, Type type, string name )
		{
			var result = container.Registrations.Transform( x => x.Any(y => y.MappedToType == type && y.Name == name) );
			return result;
		}

		public static TContainer GetRootContainer<TContainer>( this TContainer target ) where TContainer : class, IUnityContainer
		{
			var result = target.ResolveFromParent( x => x.Parent.To<TContainer>(), x => x.Parent == null, x => x );
			return result;
		}

		public static TContainer RegisterDisposable<TContainer>( this TContainer target, IDisposable disposable ) where TContainer : IUnityContainer
		{
			var extension = EnsureExtension( target );
			if ( !extension.Disposables.Contains( disposable ) )
			{
				extension.Disposables.Add( disposable );
			}
			return target;
		}

		public static TContainer DisposeAll<TContainer>( this TContainer target ) where TContainer : IUnityContainer
		{
			var extension = EnsureExtension( target );
			var container = extension.LifetimeContainer;
			var entries = target.GetLifetimeEntries().Where( x => x.Value == (object)target ).Select( x => x.Key ).ToArray();
			entries.Apply( container.Remove );
			extension.Children.NotNull( x => x.ToArray().Apply( y => y.DisposeAll() ) );
			extension.Disposables.Apply( x => x.Dispose() );
			target.Dispose();
			return target;
		}

		/*public static TContainer ConfiguredWith<TContainer>( this TContainer target, params IContainerConfigurationCommand[] commands ) where TContainer : class, IUnityContainer
		{
			EnsureExtension( target ).ConfigureWith( commands );
			return target;
		}*/

		public static TResult Create<TResult>( this IUnityContainer target, params object[] parameters )
		{
			var result = EnsureExtension( target ).Create<TResult>( parameters );
			return result;
		}
		
		public static object Create( this IUnityContainer target, Type type, params object[] parameters )
		{
			var result = EnsureExtension( target ).Create( type, parameters );
			return result;
		}

		public static IEnumerable<LifetimeEntry> GetLifetimeEntries( this IUnityContainer target )
		{
			var container = GetLifetimeContainer( target );
			var result = container.Transform( x => x.Select( ResolveLifetimeEntry ) );
			return result;
		}

		static LifetimeEntry ResolveLifetimeEntry( object x )
		{
			var resolveLifetimeEntry = x.As<ILifetimePolicy>().Transform( y => new LifetimeEntry( y, y.GetValue() ), () => new LifetimeEntry( x ) );
			return resolveLifetimeEntry;
		}
		
		public static ILifetimeContainer GetLifetimeContainer( this IUnityContainer target )
		{
			var result = EnsureExtension( target ).LifetimeContainer;
			return result;
		}

		public static IEnumerable<NamedTypeBuildKey> GetBuildKeyStack( this IUnityContainer target )
		{
			var result = EnsureExtension( target ).CurrentBuildKeyStrategy.Stack;
			return result;
		}

		static DragonSparkExtension EnsureExtension( IUnityContainer container )
		{
			var extension = container.Configure<DragonSparkExtension>();
			if ( extension == null )
			{
				container.AddExtension( extension = new DragonSparkExtension() );
			}
			return extension;
		}
	}
}