using DragonSpark.Model.Operations;

namespace DragonSpark.Compose;

public static class An
{
	public static T Instance<T>(T instance) => instance;
	public static IOperation<T> Operation<T>(IOperation<T> instance) => instance;
}