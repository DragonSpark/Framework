using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Selection.Adapters
{
	public class CommandInstanceSelector<TIn, T> : Selector<TIn, ICommand<T>>
	{
		public CommandInstanceSelector(ISelect<TIn, ICommand<T>> subject) : base(subject) {}

		public IAssign<TIn, T> ToAssignment() => new SelectedAssignment<TIn, T>(Get().Get);
	}
}