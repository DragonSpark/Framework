using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Compose;

public static class An
{
	public static T Instance<T>(T instance) => instance;
	public static IOperation<T> Operation<T>(IOperation<T> instance) => instance;
	public static IAllocated<T> Operation<T>(IAllocated<T> instance) => instance;
	public static IOperation Operation(IOperation instance) => instance;
	public static IAllocated Operation(IAllocated instance) => instance;
}