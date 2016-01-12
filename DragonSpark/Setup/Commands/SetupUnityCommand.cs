using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.TypeSystem;
using System.Linq;

namespace DragonSpark.Setup.Commands
{
	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Activate, ComponentModel.Singleton]
		public IServiceLocation Location { get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			parameter.Logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );

			Container.With( container =>
			{
				container.Registration().With( support =>
				{
					support.Instance( new ServiceLocationMonitor( Location, parameter.Locator ) );

					new object[] { parameter.Logger, Location }.Each( support.Convention );
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