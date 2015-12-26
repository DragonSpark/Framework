using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime.Values
{
	public abstract class WritableValue<T> : Value<T>, IWritableValue<T>
	{
		public abstract void Assign( T item );
	}

	class ExecutionProperty<T> : ConnectedValue<T>
	{
		public ExecutionProperty( object instance ) : base( instance, typeof( ExecutionProperty<T> ) )
		{ }
	}

	public class ExecutionContextValue<T> : WritableValue<T>
	{
		public override void Assign( T item )
		{
			new ExecutionProperty<T>( Execution.Current ).Assign( item );
		}

		public override T Item => Execution.Current.With( x => new ExecutionProperty<T>( x ).Item );
	}
}