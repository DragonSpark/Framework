using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime
{
	sealed class UnassignedValueSelector<T> : ISelect<T?, T> where T : struct
	{
		public static UnassignedValueSelector<T> Default { get; } = new UnassignedValueSelector<T>();

		UnassignedValueSelector() {}

		public T Get(T? parameter) => parameter.GetValueOrDefault();
	}
}