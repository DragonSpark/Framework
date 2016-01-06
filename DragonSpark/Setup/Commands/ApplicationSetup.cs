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
	public abstract class ApplicationSetupBase<TLocator, TLogger, TArguments> : Setup<ISetupParameter<TArguments>>
		where TLocator : IServiceLocator
		where TLogger : IMessageLogger 
	{
		[ComponentModel.Singleton]
		public IServiceLocation Location { get; set; }

		public abstract TLocator Locator { get; set; }

		[Required, ComponentModel.Singleton, Activate]
		public TLogger MessageLogger { get; set; }

		protected override void OnExecute( ISetupParameter<TArguments> parameter )
		{
			MessageLogger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			
			Location.Assign( Locator );

			base.OnExecute( parameter );
		}
	}

	public class ApplicationSetup<TLogger, TAssemblyProvider> : ApplicationSetup<TLogger, TAssemblyProvider, object>
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{}

	public class ApplicationSetup<TLogger, TAssemblyProvider, TArguments> : ApplicationSetupBase<ServiceLocator, TLogger, TArguments> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[Factory( typeof(ServiceLocatorFactory) )]
		public override ServiceLocator Locator { get; set; }

		[Required, ComponentModel.Singleton, Activate]
		public TAssemblyProvider AssemblyProvider { get; set; }

		protected override void OnExecute( ISetupParameter<TArguments> parameter )
		{
			Locator.Container.With( container =>
			{
				var assemblies = AssemblyProvider.Create();
				var support = new RegistrationSupport( container, assemblies );

				new object[] { AssemblyProvider, MessageLogger, Location }.Each( support.Convention );
			} );
			
			base.OnExecute( parameter );
		}
	}
}