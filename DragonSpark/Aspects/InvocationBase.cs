namespace DragonSpark.Aspects
{
	public abstract class InvocationBase<TParameter, TResult> : IInvocation<TParameter, TResult>
	{
		public abstract TResult Invoke( TParameter parameter );

		object IInvocation.Invoke( object parameter ) => Invoke( (TParameter)parameter );
	}
}