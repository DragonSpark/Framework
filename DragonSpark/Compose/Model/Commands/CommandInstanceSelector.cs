using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Commands;

public class CommandInstanceSelector<TIn, T> : Selector<TIn, ICommand<T>>
{
	public CommandInstanceSelector(ISelect<TIn, ICommand<T>> subject) : base(subject) {}

	public IAssign<TIn, T> ToAssignment() => new SelectedInstance<TIn, T>(Get().Get);
}