using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

sealed class IsAssigned<T> : ICondition
{
	readonly IResult<T?> _store;

	public IsAssigned(IResult<T?> store) => _store = store;

	public bool Get(None parameter) => _store.Get() is not null;
}