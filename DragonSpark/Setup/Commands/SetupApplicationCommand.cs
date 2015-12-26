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
	public abstract class SetupApplicationCommandBase<TLocator, TLogger> : SetupCommand
		where TLocator : IServiceLocator
		where TLogger : IMessageLogger
	{
		[ComponentModel.Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		public abstract TLocator Locator { get; set; }

		[Required( /*ErrorMessage = Resources.NullLoggerFacadeException*/ ), ComponentModel.Singleton, Activate]
		public TLogger MessageLogger { [return: NotNull] get; set; }

		protected override void Execute( SetupContext context )
		{
			MessageLogger.Information( Resources.LoggerCreatedSuccessfully, Priority.Low );

			MessageLogger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			
			Location.Assign( Locator );
		}
	}

	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : SetupApplicationCommandBase<ServiceLocator, TLogger> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[Factory( typeof(ServiceLocatorFactory) )]
		public override ServiceLocator Locator { get; set; }

		[Required, ComponentModel.Singleton, Activate]
		public TAssemblyProvider AssemblyProvider { [return: NotNull] get; set; }

		protected override void Execute( SetupContext context )
		{
			Locator.Container.With( container =>
			{
				var assemblies = AssemblyProvider.GetAssemblies();
				var support = new EnsuredRegistrationSupport( container, assemblies );

				new object[] { AssemblyProvider, MessageLogger, Location }.Each( support.Convention );
			} );
			
			base.Execute( context );
		}
	}
}