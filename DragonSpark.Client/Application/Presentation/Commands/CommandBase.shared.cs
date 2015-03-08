using System;
using System.Windows.Input;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Commands
{
	public abstract class CommandBase : CommandBase<object>
	{}

	public abstract class CommandBase<TParameter> : ViewObject, ICommand
	{
		public event EventHandler CanExecuteChanged = delegate {};

		protected virtual void OnCanExecuteChanged()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		void ICommand.Execute( object parameter )
		{
			var ensured = parameter.ConvertTo<TParameter>();
			Execute( ensured );
		}

		protected abstract void Execute( TParameter parameter );

		bool ICommand.CanExecute( object parameter )
		{
			var ensured = parameter.ConvertTo<TParameter>();
			var result = CanExecute( ensured );
			return result;
		}

		protected virtual bool CanExecute( TParameter parameter )
		{
			return true;
		}
	}
}