using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupApplicationCommandBase<TLocator, TLogger, TArguments> : SetupCommand<TArguments>
		where TLocator : IServiceLocator
		where TLogger : IMessageLogger 
	{
		[ComponentModel.Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		public abstract TLocator Locator { get; set; }

		[Required( /*ErrorMessage = Resources.NullLoggerFacadeException*/ ), ComponentModel.Singleton, Activate]
		public TLogger MessageLogger { [return: NotNull] get; set; }

		protected override void OnExecute( ISetupParameter<TArguments> parameter )
		{
			MessageLogger.Information( Resources.LoggerCreatedSuccessfully, Priority.Low );

			MessageLogger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			
			Location.Assign( Locator );
		}
	}

	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : SetupApplicationCommand<TLogger, TAssemblyProvider, object>
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{}

	public class SetupApplicationCommand<TLogger, TAssemblyProvider, TArguments> : SetupApplicationCommandBase<ServiceLocator, TLogger, TArguments> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[Factory( typeof(ServiceLocatorFactory) )]
		public override ServiceLocator Locator { get; set; }

		[Required, ComponentModel.Singleton, Activate]
		public TAssemblyProvider AssemblyProvider { [return: NotNull] get; set; }

		protected override void OnExecute( ISetupParameter<TArguments> parameter )
		{
			Locator.Container.With( container =>
			{
				var assemblies = AssemblyProvider.Create();
				var support = new EnsuredRegistrationSupport( container, assemblies );

				new object[] { AssemblyProvider, MessageLogger, Location }.Each( support.Convention );
			} );
			
			base.OnExecute( parameter );
		}
	}
}