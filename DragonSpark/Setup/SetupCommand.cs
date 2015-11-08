using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	public class CommandCollection : Runtime.Collection<ICommand>
	{}

	[ContentProperty( "Commands" )]
	public class CompositeSetupCommand : SetupCommand
	{
		public CommandCollection Commands { get; } = new CommandCollection();

		public override bool CanExecute( object parameter )
		{
			return base.CanExecute( parameter ) && Commands.All( command => command.CanExecute( parameter ) );
		}

		protected override void Execute( SetupContext context )
		{
			Commands.Apply( command => command.Execute( context ) );
		}
	}

	public abstract class SetupCommand : SetupCommand<SetupContext>
	{}

	public abstract class SetupCommand<TContext> : ICommand where TContext : SetupContext
	{
		readonly ConditionMonitor executed = new ConditionMonitor();

		public event EventHandler CanExecuteChanged = delegate {};

		[Default( true )]
		public bool Enabled { get; set; }

		public virtual bool CanExecute( object parameter )
		{
			return parameter is TContext && Enabled && !executed.IsApplied;
		}

		public void Execute( object parameter )
		{
			var context = parameter as TContext;
			if ( context != null )
			{
				executed.Apply( () => Execute( context ) );
			}
			else
			{
				throw new InvalidOperationException( "SetupCommand expects a SetupContext object to execute." );
			}
		}

		protected abstract void Execute( TContext context );
	}
}