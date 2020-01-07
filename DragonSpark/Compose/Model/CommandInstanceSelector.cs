using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model
{
	public class CommandInstanceSelector<TIn, T> : Selector<TIn, ICommand<T>>
	{
		public CommandInstanceSelector(ISelect<TIn, ICommand<T>> subject) : base(subject) {}

		public IAssign<TIn, T> ToAssignment() => new SelectedAssignment<TIn, T>(Get().Get);
	}
}