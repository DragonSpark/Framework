using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Extensions
{
	public static class BuilderContextExtensions
	{
		public static bool IsRegistered<T>( this IBuilderContext @this )
		{
			IPolicyList list;
			var policy = @this.Policies.Get<IBuildKeyMappingPolicy>( new NamedTypeBuildKey<T>(), out list );
			var result = policy != null;
			return result;
		}
	}

	public static class UnityContainerExtensions
	{
		static readonly IList<WeakReference<object>> BuildCache = new List<WeakReference<object>>();

		public static bool BuildUpOnce( this object target )
		{
			var result = !BuildCache.Exists( target );
			result.IsTrue( () => Services.Location.With<IObjectBuilder>( x =>
			{
				x.BuildUp( target );
				BuildCache.Add( new WeakReference<object>( target ) );
			} ) );
			return result;
		}

		/*public static TResult With<TResult>( this IUnityContainer @this, Action<TResult> action )
		{
			var result = @this.IsResolvable( typeof(TResult) ) ? @this.Resolve<TResult>().With( action ) : default(TResult);
			return result;
		}*/

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T TryResolve<T>(this IUnityContainer container)
		{
			var result = TryResolve( container, typeof(T) ) ?? default(T);
			return (T)result;
		}

		/// <summary>
		/// Utility method to try to resolve a service from the container avoiding an exception if the container cannot build the type.
		/// </summary>
		/// <param name="container">The cointainer that will be used to resolve the type.</param>
		/// <param name="typeToResolve">The type to resolve.</param>
		/// <returns>The instance of <paramref name="typeToResolve"/> built up by the container.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static object TryResolve(this IUnityContainer container, Type typeToResolve)
		{
			try
			{
				return container.Resolve(typeToResolve);
			}
			catch
			{
				return null;
			}
		}

		public static IUnityContainer EnsureRegistered<TInterface, TImplementation>( this IUnityContainer @this, LifetimeManager manager = null ) where TImplementation : TInterface
		{
			@this.IsRegistered<TInterface>().IsFalse( () => @this.RegisterType<TInterface, TImplementation>( manager ?? new ContainerControlledLifetimeManager() ) );
			return @this;
		}

		/*public static IUnityContainer FromConfiguration( this IUnityContainer container )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => container.LoadConfiguration() ) );
			return container;
		}*/

		/*public static TResult TryResolve<TResult>( this IUnityContainer target, string name = null )
		{
			try
			{
				var result = name.Transform( x => target.Resolve<TResult>( x ), () => target.Resolve<TResult>() );
				return result;
			}
			catch ( ResolutionFailedException )
			{
				return default(TResult);
			}
		}*/

		/*public static Type ResolveType<TObject>( this IUnityContainer container, string name = null )
		{
			return ResolveType( container, typeof(TObject), name );
		}

		public static Type ResolveType( this IUnityContainer container, Type type, string name = null )
		{
			var extension = container.Configure<IoCExtension>();
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
		}*/

		/*public static IEnumerable<Type> ResolveMappings<TObject>( this IUnityContainer container )
		{
			return ResolveMappings( container, typeof(TObject), null );
		}
		
		public static IEnumerable<Type> ResolveMappings<TObject>( this IUnityContainer container, string name )
		{
			return ResolveMappings( container, typeof(TObject), name );
		}

		public static IEnumerable<Type> ResolveMappings( this IUnityContainer container, Type type )
		{
			return ResolveMappings( container, type, null );
		}*/

		/*public static IEnumerable<Type> ResolveMappings( this IUnityContainer container, Type type, string name )
		{
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull( type, "type" );
			var extension = container.Configure<IoCExtension>();
			if ( extension != null )
			{
				List<Type> result;
				if ( extension.Mappings.TryGetValue( new NamedTypeBuildKey( type, name ), out result ) )
				{
					return result;
				}
			}
			return new Type[0];
		}*/

		/*public static bool IsRegisteredOrMapped<TObjectType>( this IUnityContainer container, string name = null )
		{
			var result = container.IsRegisteredOrMapped( typeof(TObjectType), name );
			return result;
		}*/

		/*public static bool IsRegisteredOrMapped( this IUnityContainer container, Type type, string name = null )
		{
			var result = container.IsRegistered( type, name );
			return result;
		}*/

		public static bool IsResolvable( this IUnityContainer container, Type type, string name = null )
		{
			var info = type.GetTypeInfo();
			var result = !info.IsInterface || container.IsRegistered( type, name );
			return result;
		}


		/*static bool IsMapped( IUnityContainer container, Type type, string name )
		{
			var result = container.Registrations.Transform( x => x.Any(y => y.MappedToType == type && y.Name == name) );
			return result;
		}*/

		/*public static TContainer GetRootContainer<TContainer>( this TContainer target ) where TContainer : class, IUnityContainer
		{
			var result = target.Loop( x => x.Parent.To<TContainer>(), x => x.Parent == null, x => x );
			return result;
		}*/

		/*public static TContainer RegisterDisposable<TContainer>( this TContainer target, IDisposable disposable ) where TContainer : IUnityContainer
		{
			var extension = EnsureExtension( target );
			if ( !extension.Disposables.Contains( disposable ) )
			{
				extension.Disposables.Add( disposable );
			}
			return target;
		}*/

		public static TContainer DisposeAll<TContainer>( this TContainer target ) where TContainer : IUnityContainer
		{
			var extension = EnsureExtension<IoCExtension>( target );
			var container = extension.LifetimeContainer;
			var entries = target.GetLifetimeEntries().Where( x => x.Value == (object)target ).Select( x => x.Key ).ToArray();
			entries.Apply( container.Remove );
			extension.Children.ToArray().Apply( y => y.DisposeAll() );
			// extension.Disposables.Apply( x => x.Dispose() );
			target.RemoveAllExtensions();
			target.Dispose();
			return target;
		}

		public static TResult Create<TResult>( this IUnityContainer target, params object[] parameters )
		{
			var result = target.Create( typeof(TResult), parameters ).To<TResult>();
			return result;
		}

		public static object Create( this IUnityContainer target, Type instanceType, params object[] parameters )
		{
			var result = EnsureExtension<IoCExtension>( target ).Create( instanceType, parameters );
			return result;
		}

		/*public static object Create( this IUnityContainer target, Type type, params object[] parameters )
		{
			var result = EnsureExtension( target ).Create( type, parameters );
			return result;
		}*/

		public static IEnumerable<LifetimeEntry> GetLifetimeEntries( this IUnityContainer target )
		{
			var container = GetLifetimeContainer( target );
			var result = container.Transform( x => x.Select( ResolveLifetimeEntry ) );
			return result;
		}

		static LifetimeEntry ResolveLifetimeEntry( object x )
		{
			var result = x.AsTo<ILifetimePolicy, LifetimeEntry>( y => new LifetimeEntry( y, y.GetValue() ), () => new LifetimeEntry( x ) );
			return result;
		}

		public static ILifetimeContainer GetLifetimeContainer( this IUnityContainer target )
		{
			var result = EnsureExtension<IoCExtension>( target ).LifetimeContainer;
			return result;
		}

		/*public static IEnumerable<NamedTypeBuildKey> GetBuildKeyStack( this IUnityContainer target )
		{
			var result = EnsureExtension( target ).CurrentBuildKeyStrategy.Stack;
			return result;
		}*/

		public static TExtension EnsureExtension<TExtension>( this IUnityContainer container ) where TExtension : UnityContainerExtension
		{
			var result = (TExtension)container.EnsureExtension( typeof(TExtension) );
			return result;
		}

		public static IUnityContainerExtensionConfigurator EnsureExtension( this IUnityContainer container, Type extensionType )
		{
			var extension = container.Configure( extensionType ) ?? container.AddExtension( Activator.Current.Activate<UnityContainerExtension>( extensionType ) ).Configure( extensionType );
			var result = (IUnityContainerExtensionConfigurator)extension;
			return result;
		}
	}
}