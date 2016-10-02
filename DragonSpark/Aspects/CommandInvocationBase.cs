namespace DragonSpark.Aspects
{
	public abstract class CommandInvocationBase<T> : InvocationBase<T, object>, IInvocation<T>
	{
		protected abstract void Execute( T parameter );

		public sealed override object Invoke( T parameter )
		{
			Execute( parameter );
			return null;
		}
	}
}