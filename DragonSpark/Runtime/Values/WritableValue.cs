using DragonSpark.Activation;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Runtime.Values
{
	public abstract class WritableValue<T> : Value<T>, IWritableValue<T>, IDisposable
	{
		public abstract void Assign( T item );

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing ) => disposing.IsTrue( OnDispose );

		protected virtual void OnDispose() {}
	}

	public class ExecutionContextValue<T> : DeferredValue<T>
	{
		public ExecutionContextValue() : base( () => new AssociatedValue<T>( Execution.Current ) ) {}
	}

	public class DeferredValue<T> : WritableValue<T>
	{
		readonly Func<IWritableValue<T>> deferred;
		
		public DeferredValue( [Required]Func<IWritableValue<T>> deferred )
		{
			this.deferred = deferred;
		}

		public override void Assign( T item ) => deferred.Use( value => value.Assign( item ) );

		public override T Item => deferred.Use( value => value.Item );
	}

	public class DecoratedValue<T> : WritableValue<T>
	{
		readonly IWritableValue<T> inner;

		public DecoratedValue( [Required]IWritableValue<T> inner )
		{
			this.inner = inner;
		}

		public override void Assign( T item ) => inner.Assign( item );

		public override T Item => inner.Item;

		protected override void OnDispose()
		{
			inner.TryDispose();
			base.OnDispose();
		}
	}
}