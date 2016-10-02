namespace DragonSpark.Commands
{
	public class DecoratedCommand : DecoratedCommand<object>
	{
		public DecoratedCommand( ICommand<object> inner ) : base( inner ) {}
	}

	public class DecoratedCommand<T> : DelegatedCommand<T>
	{
		public DecoratedCommand( ICommand<T> inner ) : base( inner.ToDelegate() ) {}
	}
}