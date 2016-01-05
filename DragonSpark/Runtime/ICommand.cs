using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System;
using System.Windows.Input;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;

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

	public abstract class AssignValueCommand<T> : DisposingCommand<T>
	{
		readonly IWritableValue<T> value;

		protected AssignValueCommand( [Required]IWritableValue<T> value )
		{
			this.value = value;
		}

		protected override void OnExecute( T parameter )
		{
			value.Assign( parameter );
		}

		protected override void OnDispose()
		{
			value.Assign( default( T ) );
			base.OnDispose();
		}
	}

	public abstract class DisposingCommand<TParameter> : Command<TParameter>, IDisposable
	{
		~DisposingCommand()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing )
		{
			disposing.IsTrue( OnDispose );
		}

		protected virtual void OnDispose()
		{}
	}

	[BuildUp]
	public abstract class Command<TParameter> : ICommand<TParameter>
	{
		public event EventHandler CanExecuteChanged = delegate {};

		public void Update() => OnUpdate();

		protected virtual void OnUpdate() => CanExecuteChanged( this, EventArgs.Empty );

		[Default( true )]
		public bool Enabled { get; set; }

		[BuildUp]
		public virtual bool CanExecute( TParameter parameter ) => !parameter.IsNull() && Enabled;

		[BuildUp]
		public void Execute( TParameter parameter ) => OnExecute( parameter );

		protected abstract void OnExecute( TParameter parameter );

		bool ICommand.CanExecute( object parameter ) => parameter.AsTo<TParameter, bool>( CanExecute );

		void ICommand.Execute( object parameter ) => parameter.AsValid<TParameter>( Execute, $"'{GetType().FullName}' expects a '{typeof( TParameter ).FullName}' object to execute." );
	}
}