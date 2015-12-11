using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using System;
using System.Globalization;
using DragonSpark.Properties;

namespace DragonSpark.Extensions
{
	public static class SetupContextExtensions
	{
		public static IUnityContainer Container( this SetupContext @this )
		{
			var result = @this.Item<IUnityContainer>();
			return result;
		}

		/*public static void RegisterTypeIfMissing<TFrom, TTo>( this SetupContext context, bool registerAsSingleton = true ) where TTo : TFrom
		{
			context.RegisterTypeIfMissing( typeof(TFrom), typeof(TTo), registerAsSingleton );
		}

		public static void RegisterTypeIfMissing( this SetupContext context, Type fromType, Type toType, bool registerAsSingleton )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( nameof( context ) );
			}

			if (fromType == null)
			{
				throw new ArgumentNullException( nameof( fromType ) );
			}
			if (toType == null)
			{
				throw new ArgumentNullException(nameof( toType ));
			}
			var container = context.Container();
			if (container.IsRegistered(fromType))
			{
				var message = string.Format(CultureInfo.CurrentCulture, Resources.TypeMappingAlreadyRegistered, fromType.Name );
				context.Logger.Information(message, Priority.Low );
			}
			else
			{
				var manager = registerAsSingleton ? (LifetimeManager)new ContainerControlledLifetimeManager() : new TransientLifetimeManager();
				container.RegisterType(fromType, toType, manager);
			}
		}*/
	}
}