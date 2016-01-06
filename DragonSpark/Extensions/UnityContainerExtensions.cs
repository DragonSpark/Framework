using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Extensions
{
	public static class UnityContainerExtensions
	{
		public static IMessageLogger Logger( this IUnityContainer @this ) => @this.Extend().Container.Resolve<IMessageLogger>();

		public static T Resolve<T>( this IUnityContainer @this, Func<T> @default ) => @this.IsRegistered<T>() ? @this.Resolve<T>() : @default();

		public static T TryResolve<T>(this IUnityContainer container) => (T)TryResolve( container, typeof( T ) );

		public static object TryResolve(this IUnityContainer container, Type typeToResolve) => new ResolutionContext( container.Logger() ).Execute( () => container.Resolve( typeToResolve ) );

		public static RegistrationSupport Registration( this IUnityContainer @this ) => @this.Registration<RegistrationSupport>();

		public static T Registration<T>( this IUnityContainer @this ) where T : RegistrationSupport => @this.Resolve<T>();

		public static IoCExtension Extend( this IUnityContainer @this ) => @this.Extension<IoCExtension>();

		public static TExtension Extension<TExtension>( this IUnityContainer container ) where TExtension : UnityContainerExtension => (TExtension)container.Extension( typeof( TExtension ) );

		public static IUnityContainerExtensionConfigurator Extension( this IUnityContainer container, Type extensionType ) => (IUnityContainerExtensionConfigurator)container.Configure( extensionType ) ?? container.Resolve( () => SystemActivator.Instance ).Activate<UnityContainerExtension>( extensionType ).WithSelf( container.AddExtension );
	}
}