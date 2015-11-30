﻿using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class UnityContainerExtensions
	{
		/*static readonly IList<WeakReference<object>> BuildCache = new List<WeakReference<object>>();

		public static bool BuildUpOnce( this object target )
		{
			var result = !BuildCache.Exists( target );
			result.IsTrue( () => Services.Location.With<IObjectBuilder>( x =>
			{
				x.BuildUp( target );
				BuildCache.Add( new WeakReference<object>( target ) );
			} ) );
			return result;
		}*/

		public static IUnityContainer RegisterInterfaces( this IUnityContainer @this, object instance )
		{
			instance.Extend().GetAllInterfaces().Each( y => @this.RegisterInstance( y, instance ) );
			return @this;
		}

		public static IUnityContainer RegisterAllClasses( this IUnityContainer @this, object instance )
		{
			instance.Extend().GetAllHierarchy().Each( y => @this.RegisterInstance( y, instance ) );
			return @this;
		}

		public static ILogger DetermineLogger( this IUnityContainer @this )
		{
			var result = @this.Resolve( () => @this.Extension<IoCExtension>().Logger );
			return result;
		}

		public static T Resolve<T>( this IUnityContainer @this, Func<T> @default )
		{
			var result = @this.IsRegistered<T>() ? @this.Resolve<T>() : @default();
			return result;
		}

		/*public static T Construct<T>( this IUnityContainer @this, params object[] parameters )
		{
			var result = @this.Resolve<IActivator>().Construct<T>( parameters );
			return result;
		}*/

		public static T Resolve<T, TDefault>( this IUnityContainer @this ) where TDefault : T
		{
			var result = (T)@this.ResolveFirst( typeof(T), typeof(TDefault) );
			return result;
		}

		public static object ResolveFirst( this IUnityContainer @this, params Type[] types )
		{
			var result = types.FirstOrDefault( @this.IsRegistered ).With( x => @this.Resolve( x ) );
			return result;
		}

		public static T TryResolve<T>(this IUnityContainer container)
		{
			var result = TryResolve( container, typeof(T) );
			return (T)result;
		}

		public static object TryResolve(this IUnityContainer container, Type typeToResolve)
		{
			var result = new ResolutionContext( container.DetermineLogger() ).Execute( () => container.Resolve( typeToResolve ) );
			return result;
		}

		public static IUnityContainer EnsureRegistered<TInterface, TImplementation>( this IUnityContainer @this, LifetimeManager manager = null ) where TImplementation : TInterface
		{
			@this.IsRegistered<TInterface>().IsFalse( () => @this.RegisterType<TInterface, TImplementation>( manager ?? new TransientLifetimeManager() ) );
			return @this;
		}

		public static IUnityContainer EnsureRegistered<TInterface>( this IUnityContainer @this, Func<TInterface> instance, LifetimeManager manager = null )
		{
			@this.IsRegistered<TInterface>().IsFalse( () => @this.RegisterInstance( instance() ) );
			return @this;
		}

		/*public static TContainer GetRootContainer<TContainer>( this TContainer target ) where TContainer : class, IUnityContainer
		{
			var result = target.Loop( x => x.Parent.To<TContainer>(), x => x.Parent == null, x => x );
			return result;
		}*/

		/*public static IEnumerable<NamedTypeBuildKey> GetBuildKeyStack( this IUnityContainer target )
		{
			var result = EnsureExtension( target ).CurrentBuildKeyStrategy.Stack;
			return result;
		}*/

		/*public static TResult Extension<TExtension, TResult>( this IUnityContainer @this, With<TExtension, TResult> @with ) where TExtension : UnityContainerExtension
		{
			var extension = @this.EnsureExtension<TExtension>();
			var result = extension.Transform( x => with( x ) );
			return result;
		}*/

		public static TExtension Extension<TExtension>( this IUnityContainer container ) where TExtension : UnityContainerExtension
		{
			var result = (TExtension)container.Extension( typeof(TExtension) );
			return result;
		}

		public static IUnityContainerExtensionConfigurator Extension( this IUnityContainer container, Type extensionType )
		{
			var extension = container.Configure( extensionType ) ?? container.Resolve( () => SystemActivator.Instance ).Activate<UnityContainerExtension>( extensionType ).WithSelf( container.AddExtension );
			var result = (IUnityContainerExtensionConfigurator)extension;
			return result;
		}

		/*static IUnityContainerExtensionConfigurator CreateExtension( IUnityContainer container, Type extensionType )
		{
			var extension = extensionType == typeof(IoCExtension) ? (UnityContainerExtension)new Activator( container ).Construct( extensionType ) : ;
			var result = (IUnityContainerExtensionConfigurator)container.AddExtension( extension ).Configure( extensionType );
			return result;
		}*/
	}
}