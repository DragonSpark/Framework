using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
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

	public class AssignValueCommand<T> : DisposingCommand<T>
	{
		readonly IWritableValue<T> value;
		readonly T current;

		public AssignValueCommand( [Required]IWritableValue<T> value ) : this( value, value.Item ) {}

		public AssignValueCommand( [Required]IWritableValue<T> value, T current )
		{
			this.value = value;
			this.current = current;
		}

		protected override void OnExecute( T parameter )
		{
			value.Assign( parameter );
		}

		protected override void OnDispose()
		{
			value.Assign( current );
			value.TryDispose();
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

	public class DecoratedCommand<T> : Command<T>
	{
		readonly ICommand<T> inner;

		public DecoratedCommand( [Required]ICommand<T> inner )
		{
			this.inner = inner;
		}

		public override bool CanExecute( T parameter ) => inner.CanExecute( parameter );

		protected override void OnExecute( T parameter ) => inner.Execute( parameter );
	}

	public abstract class Command<TParameter> : ICommand<TParameter>
	{
		public event EventHandler CanExecuteChanged = delegate {};

		public void Update() => OnUpdate();

		protected virtual void OnUpdate() => CanExecuteChanged( this, EventArgs.Empty );

		[BuildUp]
		public virtual bool CanExecute( TParameter parameter ) => !parameter.IsNull();

		[BuildUp]
		public void Execute( TParameter parameter ) => OnExecute( parameter );

		protected abstract void OnExecute( TParameter parameter );

		bool ICommand.CanExecute( object parameter ) => parameter.AsTo<TParameter, bool>( CanExecute );

		void ICommand.Execute( object parameter ) => parameter.AsValid<TParameter>( Execute, $"'{GetType().FullName}' expects a '{typeof( TParameter ).FullName}' object to execute." );
	}
}