using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;

namespace DragonSpark.Operations
{
	public class TaskSelector<_, T> : Selector<_, Task<T>>
	{
		public TaskSelector(ISelect<_, Task<T>> subject) : base(subject) {}

		public OperationSelector<_, T> Promote()
			=> new OperationSelector<_, T>(Get().Select(SelectOperation<T>.Default));

		public TaskSelector<_, TTo> Then<TTo>(ISelect<T, TTo> select) => Then(select.Get);

		public TaskSelector<_, TTo> Then<TTo>(Func<T, TTo> select)
			=> new TaskSelector<_, TTo>(Get().Select(new Selection<T, TTo>(select)));
	}
}