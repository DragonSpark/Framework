using System;
using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocatorFactory : FactoryBase<IFixture, IServiceLocator>
	{
		public static ServiceLocatorFactory Instance { get; } = new ServiceLocatorFactory();

		protected override IServiceLocator CreateFrom( Type resultType, IFixture parameter )
		{
			var container = new UnityContainer().RegisterInstance( parameter );
			var logger = parameter.GetLogger().With( rl => container.RegisterInstance<IRecordingLogger>( rl ) ) ?? container.Extension<IoCExtension>().Logger;
			container.RegisterInstance( logger );
			var result = new ServiceLocator( container );
			return result;
		}
	}
}