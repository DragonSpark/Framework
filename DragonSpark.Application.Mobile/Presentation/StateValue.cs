using DragonSpark.Model.Selection;
using Uno.Extensions.Reactive;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class StateValue<T, TValue> : ISelect<StateValueInput<T, TValue>, IState<TValue>>
    where T : class where TValue : notnull
{
    public static StateValue<T, TValue> Default { get; } = new();

    StateValue() {}

    public IState<TValue> Get(StateValueInput<T, TValue> parameter)
    {
        var (owner, subject) = parameter;
        return State.Value(owner, subject);
    }
}