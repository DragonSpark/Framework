using DragonSpark.Extensions;
using System;
using System.Windows.Input;
using DragonSpark.Coercion;

namespace DragonSpark.Commands
{
	public abstract class CommandBase<T> : ICommand<T>
	{
		public event EventHandler CanExecuteChanged = delegate {};

		readonly ICoercer<T> coercer;

		protected CommandBase() : this( Coercer<T>.Default ) {}
		protected CommandBase( ICoercer<T> coercer )
		{
			this.coercer = coercer;
		}

		public virtual void Update() => CanExecuteChanged( this, EventArgs.Empty );

		bool ICommand.CanExecute( object parameter ) => IsSatisfiedBy( coercer.Coerce( parameter ) );
		void ICommand.Execute( object parameter ) => Execute( coercer.Coerce( parameter ) );

		public virtual bool IsSatisfiedBy( T parameter ) => parameter.IsAssigned();
		public abstract void Execute( T parameter );
	}
}