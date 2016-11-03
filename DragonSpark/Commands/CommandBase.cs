using DragonSpark.Extensions;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using System;
using System.Windows.Input;

namespace DragonSpark.Commands
{
	public abstract class CommandBase<T> : ICommand<T>
	{
		public event EventHandler CanExecuteChanged = delegate {};

		readonly IParameterizedSource<T> coercer;

		protected CommandBase() : this( Coercer<T>.Default ) {}
		protected CommandBase( IParameterizedSource<T> coercer )
		{
			this.coercer = coercer;
		}

		public virtual void Update() => CanExecuteChanged( this, EventArgs.Empty );

		bool ICommand.CanExecute( object parameter ) => IsSatisfiedBy( coercer.Get( parameter ) );
		void ICommand.Execute( object parameter ) => Execute( coercer.Get( parameter ) );

		public virtual bool IsSatisfiedBy( T parameter ) => parameter.IsAssigned();
		public abstract void Execute( T parameter );
	}
}