namespace DragonSpark.Activation
{
	public interface IFactory<out T> : IFactory
	{
		T Create();
	}

	public interface IFactory<in TParameter, out TResult> : IFactory<TResult>
	{
		TResult Create( TParameter parameter );
	}

	public abstract class FactoryBase<TResult> : FactoryBase<object, TResult> where TResult : class
	{}

	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult> where TResult : class
	{
		protected abstract TResult CreateFrom( TParameter parameter );

		public TResult Create()
		{
			return Create( default(TParameter) );
		}

		public TResult Create( TParameter parameter )
		{
			var result = CreateFrom( parameter );
			return result;
		}

		object IFactory.Create( object parameter )
		{
			var result = Create( parameter is TParameter ? (TParameter)parameter : default(TParameter) );
			return result;
		}
	}
}