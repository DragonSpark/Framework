using System;
using System.Globalization;
using Microsoft.Practices.Unity;
using Prism.Logging;

namespace Prism.Unity
{
	public static class SetupContextExtensions
	{
		public static IUnityContainer Container( this SetupContext @this )
		{
			var result = @this.Item<IUnityContainer>();
			return result;
		}

		/// <summary>
		/// Registers a type in the container only if that type was not already registered.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="fromType">The interface type to register.</param>
		/// <param name="toType">The type implementing the interface.</param>
		/// <param name="registerAsSingleton">Registers the type as a singleton.</param>
		public static void RegisterTypeIfMissing( this SetupContext context, Type fromType, Type toType, bool registerAsSingleton)
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if (fromType == null)
			{
				throw new ArgumentNullException("fromType");
			}
			if (toType == null)
			{
				throw new ArgumentNullException("toType");
			}
			if (context.Container().IsTypeRegistered(fromType))
			{
				var message = string.Format(CultureInfo.CurrentCulture,
					Properties.Resources.TypeMappingAlreadyRegistered,
					fromType.Name);
				context.Logger.Log(message, Category.Debug, Priority.Low);
			}
			else
			{
				var container = context.Container();
				var manager = registerAsSingleton ? (LifetimeManager)new ContainerControlledLifetimeManager() : new TransientLifetimeManager();
				container.RegisterType(fromType, toType, manager);
			}
		}
	}
}