using System;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Activation.FactoryModel
{
	public interface IFactory
	{
		object Create();
	}

	public interface IFactory<out T> : IFactory
	{
		new T Create();
	}

	public interface ITransformer<T> : IFactory<T, T>
	{}

	public interface IFactory<in TParameter, out TResult> : IFactoryWithParameter
	{
		TResult Create( TParameter parameter );
	}

	public static class FactoryExtensions
	{
		class Delegate<T, U> : ConnectedValue<Func<T, U>>
		{
			public Delegate( IFactory<T, U> instance ) : base( instance, typeof(Delegate<T, U>), () => instance.Create )
			{}
		}

		public static Func<T, U> ToDelegate<T, U>( this IFactory<T, U> @this ) => new Delegate<T,U>( @this ).Item;
	}
}