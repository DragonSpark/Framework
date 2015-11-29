using System;
using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using UnityConventionRegistrationService = DragonSpark.Windows.Setup.UnityConventionRegistrationService;

namespace DragonSpark.Testing.Framework
{
	public class UnityConventionRegistrationServiceFactory : FactoryBase<IFixture, IConventionRegistrationService>
	{
		public static UnityConventionRegistrationServiceFactory Instance { get; } = new UnityConventionRegistrationServiceFactory();

		protected override IConventionRegistrationService CreateFrom( Type resultType, IFixture parameter )
		{
			return parameter.GetLocator().Transform( locator =>
			{
				return locator.Transform( x => x.GetInstance<IUnityContainer>(), parameter.TryCreate<IUnityContainer> ).Transform( container =>
				{
					var logger = parameter.GetLogger() ?? container.Resolve<ILogger>() ?? DebugLogger.Instance;
					var activator = locator.GetInstance<IActivator>() ?? parameter.TryCreate<IActivator>() ?? container.Extension<IoCExtension>().CreateActivator();
					var result = new UnityConventionRegistrationService( container, activator, logger );
					return result;
				});
			} );
		}
	}
}