using System;
using System.Windows.Input;

namespace Prism
{
	public abstract class SetupCommand : SetupCommand<SetupContext>
	{}

	public abstract class SetupCommand<TContext> : ICommand where TContext : SetupContext
	{
		public event EventHandler CanExecuteChanged = delegate {};

		public virtual bool CanExecute( object parameter )
		{
			return parameter is TContext;
		}

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