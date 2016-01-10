using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupApplicationCommandBase<TLogger> : SetupApplicationCommandBase<ServiceLocator, TLogger>
		where TLogger : IMessageLogger
	{ }

	public abstract class SetupApplicationCommandBase<TLocator, TLogger> : SetupCommand
		where TLocator : IServiceLocator
		where TLogger : IMessageLogger 
	{
		[Required, Singleton, Activate]
		public virtual IServiceLocation Location { get; set; }

		[Required, Singleton, Factory, Activate]
		public virtual TLocator Locator { get; set; }

		[Required, Singleton, Activate]
		public virtual TLogger MessageLogger { get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			parameter.AsRegistered( MessageLogger ).Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			
			Location.Assign( Locator );
		}
	}
}