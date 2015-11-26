using System;

namespace DragonSpark.Activation
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

	public interface IFactoryWithParameter
	{
		object Create( object parameter );

		/*Type ParameterType { get; }*/
	}

	public interface IFactory<in TParameter, out TResult> : IFactoryWithParameter
	{
		TResult Create( TParameter parameter );
	}
}