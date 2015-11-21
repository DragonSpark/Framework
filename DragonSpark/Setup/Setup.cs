using DragonSpark.Extensions;
using DragonSpark.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;
using DragonSpark.Diagnostics;

namespace DragonSpark.Setup
{
	[ContentProperty( "Commands" )]
	public abstract class Setup : ISetup
	{
		protected Setup()
		{
			commands = new Lazy<Collection<ICommand>>( DetermineDefaultCommands );
		}

	    public Collection<object> Items { get; } = new Collection<object>();

		public Collection<ICommand> Commands => commands.Value;
		readonly Lazy<Collection<ICommand>> commands;

		protected virtual Collection<ICommand> DetermineDefaultCommands()
		{
			var result = new Collection<ICommand>();
			return result;
		}

		protected virtual ICommand Prepare( ICommand command )
		{
			return command.WithDefaults();
		}

		public virtual void Run(object arguments = null)
		{
			var context = CreateContext( arguments );

			foreach ( var command in DetermineRunCommands( context ) )
			{
				command.Execute( context );
			}
		}

		protected virtual IEnumerable<ICommand> DetermineRunCommands( SetupContext context )
		{
			return Commands.Select( Prepare ).Where( command => command.CanExecute( context ) );
		}

		protected virtual SetupContext CreateContext( object arguments )
		{
			var result = new SetupContext( arguments );
			return result;
		}
	}

	public abstract class Setup<TLogger, TModuleCatalog> : Setup
		where TLogger : ILogger, new()
		where TModuleCatalog : IModuleCatalog, new()
	{
		protected override Collection<ICommand> DetermineDefaultCommands()
		{
			var result = base.DetermineDefaultCommands();
			result.AddRange( new ICommand[]
			{
				new SetupLoggingCommand<TLogger>(),
				new SetupModuleCatalogCommand<TModuleCatalog>(),
				new SetupUnityCommand(),
				new RegisterFrameworkExceptionTypesCommand()
			} );
			return result;
		}
	}
}
