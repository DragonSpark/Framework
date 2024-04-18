using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.State;

public sealed class IsActive : ICondition<object>
{
	public static IsActive Default { get; } = new();

	IsActive() : this(ActiveState.Default) {}

	readonly ISelect<object, bool> _store;

	public IsActive(ISelect<object, bool> store) => _store = store;

	public bool Get(object parameter) => parameter is IActivityReceiver ar ? ar.Active : _store.Get(parameter);
}