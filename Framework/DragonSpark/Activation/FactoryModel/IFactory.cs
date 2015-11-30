namespace DragonSpark.Activation.FactoryModel
{
	public interface IFactory
	{
		object Create();

		/*Type ResultType { get; }*/
	}

	public interface IFactory<out T> : IFactory
	{
		new T Create();
	}

	public interface IFactory<in TParameter, out TResult> : IFactoryWithParameter
	{
		TResult Create( TParameter parameter );
	}
}