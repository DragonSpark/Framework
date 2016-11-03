using DragonSpark.Activation;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonSpark.Composition
{
	public class ServiceProviderConfigurations : Application.Setup.ServiceProviderConfigurations
	{
		public static ServiceProviderConfigurations Default { get; } = new ServiceProviderConfigurations();
		ServiceProviderConfigurations() : this( ServiceProviderSource.Default.Get ) {}

		readonly Func<IActivator> source;

		[UsedImplicitly]
		protected ServiceProviderConfigurations( Func<IActivator> source ) : this( source, InitializeExportsCommand.Default.Execute ) {}

		ServiceProviderConfigurations( Func<IActivator> source, Action<IActivator> configure )
		{
			this.source = new ConfiguringFactory<IActivator>( source, configure ).Get;
		}

		protected override IEnumerable<ICommand> Yield()
		{
			yield return Application.Setup.ActivatorFactory.Default.ToCommand( source );
			foreach ( var command in base.Yield() )
			{
				yield return command;
			}
		}
	}
}