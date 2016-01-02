using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System;
using System.Windows.Input;

namespace DragonSpark.Runtime
{
	public interface ICommand<in TParameter> : System.Windows.Input.ICommand
	{
		bool CanExecute( TParameter parameter );

		void Execute( TParameter parameter );

		void Update();
	}

	public abstract class Command : Command<object>
	{}

	[BuildUp]
	public abstract class Command<TParameter> : ICommand<TParameter>
	{
		public event EventHandler CanExecuteChanged = delegate {};

		public void Update()
		{
			OnUpdate();
		}

		protected virtual void OnUpdate()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		[Default( true )]
		public bool Enabled { get; set; }

		[BuildUp]
		bool ICommand.CanExecute( object parameter )
		{
			var result = parameter.AsTo<TParameter, bool>( CanExecute );
			return result;
		}

		public virtual bool CanExecute( TParameter parameter )
		{
			var result = !parameter.IsNull() && Enabled;
			return result;
		}

		void ICommand.Execute( object parameter )
		{
			parameter.AsValid<TParameter>( Execute, $"'{GetType().FullName}' expects a '{typeof( TParameter ).FullName}' object to execute." );
		}

		[BuildUp]
		public void Execute( TParameter parameter )
		{
			OnExecute( parameter );
		}

		protected abstract void OnExecute( TParameter parameter );
	}
}