using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Commands;

public class CommandInstanceComposer<TIn, T> : Composer<TIn, ICommand<T>>
{
	public CommandInstanceComposer(ISelect<TIn, ICommand<T>> subject) : base(subject) {}

	public IAssign<TIn, T> ToAssignment() => new SelectedInstance<TIn, T>(Get().Get);
}