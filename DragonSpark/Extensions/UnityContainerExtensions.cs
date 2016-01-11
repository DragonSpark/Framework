using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Extensions
{
	public static class UnityContainerExtensions
	{
		public static IServiceRegistry Registry( this IUnityContainer @this ) => @this.Resolve<IServiceRegistry>();

		public static IMessageLogger Logger( this IUnityContainer @this ) => @this.Resolve<IMessageLogger>( () => @this.Extend().Logger );

		public static T Resolve<T>( this IUnityContainer @this, Func<T> @default ) => @this.IsRegistered<T>() ? @this.Resolve<T>() : @default();

		public static T TryResolve<T>(this IUnityContainer container) => (T)TryResolve( container, typeof(T) );

		public static object TryResolve(this IUnityContainer container, Type typeToResolve, string name = null ) => container.Resolve<ResolutionContext>().Execute( () => container.Resolve( typeToResolve, name ) );

		public static RegistrationSupport Registration( this IUnityContainer @this ) => @this.Registration<RegistrationSupport>();

		public static T Registration<T>( this IUnityContainer @this ) where T : RegistrationSupport => @this.Resolve<T>();

		public static IoCExtension Extend( this IUnityContainer @this ) => @this.Extension<IoCExtension>();

		public static TExtension Extension<TExtension>( this IUnityContainer container ) where TExtension : UnityContainerExtension => (TExtension)container.Extension( typeof(TExtension) );

		public static IUnityContainerExtensionConfigurator Extension( this IUnityContainer container, Type extensionType )
		{
			var configure = container.Configure( extensionType );
			return (IUnityContainerExtensionConfigurator)configure ?? container.Resolve<IActivator>( () => SystemActivator.Instance ).Activate<UnityContainerExtension>( extensionType ).WithSelf( container.AddExtension );
		}
	}
}