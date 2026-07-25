using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Results;

sealed class Delegates<T> : ReferenceValueStore<IResult<T>, Func<T>>
{
	public static Delegates<T> Default { get; } = new();

	Delegates() : base(x => x.Get) {}
}