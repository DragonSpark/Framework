using DragonSpark.Model.Operations;

namespace DragonSpark.Compose
{
	public static class An
	{
		public static IOperation<T> Operation<T>(IOperation<T> instance) => instance;
		public static ISelecting<TIn, TOut> OperationResult<TIn, TOut>(ISelecting<TIn, TOut> instance) => instance;
	}
}