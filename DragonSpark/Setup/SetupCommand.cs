using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using System;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	public abstract class SetupCommand : SetupCommand<SetupContext>
	{}

	[BuildUp]
	public abstract class SetupCommand<TContext> : ICommand where TContext : SetupContext
	{
		public event EventHandler CanExecuteChanged = delegate {};

		public void Update()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		[Default( true )]
		public bool Enabled { get; set; }

		public virtual bool CanExecute( object parameter )
		{
			return parameter is TContext && Enabled;
		}

		[BuildUp]
		public void Execute( object parameter )
		{
			var context = parameter as TContext;
			if ( context != null )
			{
				Execute( context );
			}
			else
			{
				throw new InvalidOperationException( "SetupCommand expects a SetupContext object to execute." );
			}
		}

		protected abstract void Execute( TContext context );
	}
}