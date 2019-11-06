using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;

namespace DragonSpark.Operations
{
	public class OperationSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public OperationSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Demote() => new TaskSelector<_, T>(Get().Select(SelectTask<T>.Default));

		public TaskSelector<_, TTo> Then<TTo>(ISelect<T, TTo> select) => Then(select.Get);

		public TaskSelector<_, TTo> Then<TTo>(Func<T, TTo> select) => Demote().Then(select);
	}
}