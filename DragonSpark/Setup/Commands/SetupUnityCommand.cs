using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;
using System.Linq;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupUnityCommand<TAssemblyProvider> : ConfigureUnityCommand where TAssemblyProvider : IAssemblyProvider
	{
		[Activate, ComponentModel.Singleton]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocator Locator { get; set; }

		[Required, ComponentModel.Singleton, Activate]
		public virtual TAssemblyProvider AssemblyProvider { get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			var logger = parameter.Item<IMessageLogger>();
			logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );

			Container.With( container =>
			{
				container.Registry().RegisterFactory( AssemblyProvider );

				container.Registration().With( support =>
				{
					support.Instance( new ServiceLocationMonitor( Location, Locator ) );

					new object[] { AssemblyProvider, logger, Location }.Each( support.Convention );
				} );

				container.Registration<EnsuredRegistrationSupport>().With( ensured =>
				{
					parameter.Append( parameter.Items ).Except( container.ToItem() ).Each( ensured.Convention );
				} );
			} );

			base.OnExecute( parameter );
		}
	}
}