using DragonSpark.ComponentModel;
using System;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	public abstract class SetupCommand : SetupCommand<SetupContext>
	{}

	public abstract class SetupCommand<TContext> : ICommand where TContext : SetupContext
	{
		readonly ConditionMonitor executed = new ConditionMonitor();

		public event EventHandler CanExecuteChanged = delegate {};

		public void Update()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

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