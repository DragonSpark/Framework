using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Activation;

sealed class Singleton<T> : Deferred<T>, ISingleton<T>
{
	public static Singleton<T> Default { get; } = new Singleton<T>();

	Singleton() : base(Singletons.Default.Then().Bind<T>()) {}
}